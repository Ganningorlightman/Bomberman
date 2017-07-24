using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    private Rigidbody rigidBody;
    private Transform myTransform;
    public GameObject bomb;
    private CharacterController charContr;
    private float distlLenght = 1.5f;
    private Animator ani;
    private GameInitializer gameIni;

    public LayerMask wallLayer;
    public LayerMask wWallLayer;
    public LayerMask bombLayer;

    public float moveSpeed;
    public int Bombs;
    private int BombsCounter = 0;
    public int Flames;
    public bool Wallpass;
    public bool Bombpass;
    public bool Flamepass;
    public bool Detonator;
    public bool Death { get; set; }
    public Vector3 PlayerPosition { get { return Rounding(myTransform.position.x, myTransform.position.z); } }

    void Start() {
        Death = false;
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        gameIni = GameObject.FindObjectOfType<GameInitializer>();
        charContr = GetComponent<CharacterController>();
        ani = GetComponent<Animator>();
        moveSpeed = PlayerCharacteristics.MoveSpeed;
        Bombs = PlayerCharacteristics.Bombs;
        Flames = PlayerCharacteristics.Flames;
        Wallpass = PlayerCharacteristics.WallPass;
        Bombpass = PlayerCharacteristics.BombPass;
        Flamepass = PlayerCharacteristics.FlamePass;
        Detonator = PlayerCharacteristics.Detonator; 
    }

    void Update()
    {
        if (!Death)
        {
            PlayerMovement();
            
            if (Input.GetKeyDown(KeyCode.P))                        
                DropBomb();          
        }
    }

    private void PlayerMovement()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { //Вверх          
            myTransform.rotation = Quaternion.Euler(0, 0, 0);
            Move();
        } else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { //Вниз    
            myTransform.rotation = Quaternion.Euler(0, 180, 0);
            Move();
        } else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { //направо
            myTransform.rotation = Quaternion.Euler(0, 90, 0);
            Move();
        } else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { //налево
            myTransform.rotation = Quaternion.Euler(0, 270, 0);
            Move();
        } else ani.SetBool("Speed", false);
    }

    private void Move()
    {
        if(CheckLayers()) {
            rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
            gameIni.cameraPosition += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
        }
            ani.SetBool("Speed", true);           
    }
    private Vector3 Transformation(Vector3 old) {
        return new Vector3(Mathf.CeilToInt(-old.x / GameInitializer.BlockSize), 0, Mathf.CeilToInt(old.z / GameInitializer.BlockSize));
    }
    private Vector3 Rounding(float x, float z) {
        x = Mathf.RoundToInt(x);
        z = Mathf.RoundToInt(z);
        var Size = GameInitializer.BlockSize;
        if((Mathf.Abs(x) % Size) >= (int)(Size / 2))
            x = x - (x % Size) - Size;
        else x = x - (x % Size);
        if((Mathf.Abs(z) % Size) >= (int)(Size / 2))
            z = z - (z % Size) + Size;
        else z = z - (z % Size);
        return new Vector3(x, 0f, z);
    }
    private bool CheckLayers()
    {
        RaycastHit hit;
        Vector3 p1 = myTransform.position + charContr.center + Vector3.up * -charContr.height * 0.5F;
        Vector3 p2 = p1 + Vector3.up * charContr.height;

        if ((!Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, distlLenght, wallLayer)) && Wallpass && Bombpass)
        {
            return true;
        }
        else if ((!Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, distlLenght, wallLayer)) && (!Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, distlLenght, wWallLayer)) && !Wallpass && Bombpass)
            {
            return true;
            }
        else if ((!Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, distlLenght, wallLayer)) && (!Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, distlLenght, bombLayer)) && Wallpass && !Bombpass)
        {
            return true;
        }
        else if ((!Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, distlLenght, wallLayer)) && (!Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, distlLenght, bombLayer)) && (!Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, distlLenght, wWallLayer)) && !Wallpass && !Bombpass)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void DropBomb()
    {
        if (BombsCounter < Bombs) {
            BombsCounter++;            
            bomb = ObjectLoader.GetObject("Models/Bomb");           
            bomb.transform.position = Rounding(myTransform.position.x, myTransform.position.z);
            gameIni = GameObject.FindObjectOfType<GameInitializer>();
            Vector3 old = new Vector3(bomb.transform.position.x, bomb.transform.position.y, bomb.transform.position.z);
            old = Transformation(old);
            gameIni.Map.ChangeCellUnitType(old.x, old.z, UnitType.Bomb);
            ani.SetTrigger("Plant");
            var bombObject = Instantiate(bomb);
            Bomb bombBehavior = bombObject.GetComponent<Bomb>();
            bombBehavior.Flames = Flames;
            if (Detonator) bombBehavior.Detonator = true;
            bombBehavior.Initialized(() => { BombsCounter--; if (BombsCounter < 0) BombsCounter = 0; });
        }
    }
    private void Dead()
    {
        Death = true;
        ani.SetTrigger("Death");
        GetComponent<Collider>().enabled = false;
        print("Defeat");
        Destroy(gameObject, 4f);       
    }
    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy") && (!Death))
        {
            Dead();
        }

        if (col.CompareTag("Explosion") && (!Flamepass) && (!Death))
        {
            Dead();
        }

        if (col.CompareTag("Bombs"))
        {
            Bombs++;
            Destroy(col.gameObject);         
        }

        if (col.CompareTag("Flames"))
        {
            Flames++;
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Speed"))
        {
            moveSpeed += 2f;
            Destroy(col.gameObject);
        }

        if (col.CompareTag("WallPass"))
        {
            Wallpass = true;
            Destroy(col.gameObject);
        }

        if (col.CompareTag("BombPass"))
        {
            Bombpass = true;
            Destroy(col.gameObject);
        }

        if (col.CompareTag("FlamePass"))
        {
            Flamepass = true;
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Detonator"))
        {
            Detonator = true;
            Destroy(col.gameObject);
        }

        if ((col.CompareTag("Exit")) && (GameController.ExitOpen))
        {
            print("Win");
            PlayerCharacteristics.MoveSpeed = (int)moveSpeed;
            PlayerCharacteristics.Bombs = Bombs;
            PlayerCharacteristics.Flames = Flames;
            PlayerCharacteristics.WallPass = Wallpass;
            PlayerCharacteristics.BombPass = Bombpass;
            PlayerCharacteristics.FlamePass = Flamepass;
            PlayerCharacteristics.Detonator = Detonator;
            GameController.ExitCreated = false;
            GameController.Level++;
            Destroy(col.gameObject);
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
        }
    }

}
