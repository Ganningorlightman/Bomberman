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
    private bool dead = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        charContr = GetComponent<CharacterController>();
        ani = GetComponent<Animator>();
        moveSpeed = PlayerCharacteristics.MoveSpeed;
        Bombs = PlayerCharacteristics.Bombs;
        Flames = PlayerCharacteristics.Flames;
        Wallpass = PlayerCharacteristics.WallPass;
        Bombpass =  PlayerCharacteristics.BombPass;
        Flamepass = PlayerCharacteristics.FlamePass;
        Detonator = PlayerCharacteristics.Detonator;
}

    void Update()
    {
        if (!dead)
        {
            PlayerMovement();
            
            if (Input.GetKeyDown(KeyCode.P))                        
                DropBomb();          
        }
    }

    private void PlayerMovement()
    {       
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        { //В верх          
            myTransform.rotation = new Quaternion(0, 0, 0, 0);
            Move();
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        { //В низ    
            myTransform.rotation = new Quaternion(0, 90, 0, 0);
            Move();
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        { //на право
            myTransform.rotation = new Quaternion(0, -90, 0, 90);
            Move();
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        { //на лево
            myTransform.rotation = new Quaternion(0, 90, 0, 90);
            Move();
        }
        else ani.SetBool("Speed", false);

    }

    private void Move()
    {
        if (CheckLayers())        
            rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
            ani.SetBool("Speed", true);           
    }

    private bool CheckLayers()
    {
        RaycastHit hit;
        Vector3 p1 = transform.position + charContr.center + Vector3.up * -charContr.height * 0.5F;
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
            var BombSize = 5;
            bomb = ObjectLoader.GetObject("Models/Bomb");
            float x = Mathf.RoundToInt(myTransform.position.x);
            float z = Mathf.RoundToInt(myTransform.position.z);          
            if ((Mathf.Abs(x) % BombSize) >= (int)(BombSize / 2))
                x = x - (x % BombSize) - BombSize;
            else x = x - (x % BombSize);
            if ((Mathf.Abs(z) % BombSize) >= (int)(BombSize / 2))
                z = z - (z % BombSize) + BombSize;
            else z = z - (z % BombSize);
            bomb.transform.position = new Vector3(x, 0f, z);          
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
        dead = true;
        ani.SetTrigger("Death");
        Destroy(gameObject, 4f);
        GetComponent<Collider>().enabled = false;
        print("Defeat");
    }
  
    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy") && (!dead))
        {
            Dead();
        }

        if (col.CompareTag("Explosion") && (!Flamepass) && (!dead))
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
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
        }
    }

}
