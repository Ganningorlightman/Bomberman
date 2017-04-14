using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody rigidBody;
    private Transform myTransform;
    private Vector3 target;
    public GameObject bomb;
    private CharacterController charContr;
    public float distlLenght = 1.5f;
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

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        charContr = GetComponent<CharacterController>();
    }

    void Update()
    {  
        UpdatePlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropBomb();
        }
    }

    private void UpdatePlayerMovement()
    {       
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        { //В верх          
            myTransform.rotation = new Quaternion(0, 0, 0, 0);
            Move();
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        { //В низ    
            myTransform.rotation = new Quaternion(0, 90, 0, 0);
            Move();
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        { //на право
            myTransform.rotation = new Quaternion(0, -90, 0, 90);
            Move();
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        { //на лево
            myTransform.rotation = new Quaternion(0, 90, 0, 90);
            Move();
        }
        
    }

    private void Move()
    {
        if (CheckLayers())
            rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
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
            bomb = ObjectLoader.getObject("Models/Bomb");
            bomb.transform.localScale = new Vector3(5f, 5f, 5f);
            bomb.transform.position = new Vector3(Mathf.RoundToInt(myTransform.position.x), 0f, Mathf.RoundToInt(myTransform.position.z));
            var bombObject = Instantiate(bomb);
            Bomb bombBehavior = bombObject.GetComponent<Bomb>();
            bombBehavior.Flames = Flames;
            if (Detonator) bombBehavior.Detonator = true;
            bombBehavior.Initialized(() => { BombsCounter--; if (BombsCounter < 0) BombsCounter = 0; });
        }
    }
  
    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }

        if (col.CompareTag("Explosion") && !Flamepass)
        {
            Destroy(gameObject);
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
            moveSpeed += 5f;
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
    }

}
