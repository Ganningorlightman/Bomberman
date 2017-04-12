using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 10f;

    private Rigidbody rigidBody;
    private Transform myTransform;
    private Vector3 target;
    public GameObject bomb;
    private CharacterController charContr;
    public float distlLenght = 1.5f;
    public LayerMask wallLayer;
    public LayerMask wWallLayer;
    public LayerMask bombLayer;
    public int Bombs;
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
            if (CheckLayers())
                rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        { //В низ    
            myTransform.rotation = new Quaternion(0, 90, 0, 0);
            if (CheckLayers())
                rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        { //на право
            myTransform.rotation = new Quaternion(0, -90, 0, 90);
            if (CheckLayers())
                rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        { //на лево
            myTransform.rotation = new Quaternion(0, 90, 0, 90);
            if (CheckLayers())
                rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
        }
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
        bomb = (Resources.Load("Models/Bomb", typeof(GameObject))) as GameObject;
        bomb.transform.localScale = new Vector3(5f, 5f, 5f);
        bomb.transform.position = new Vector3(Mathf.RoundToInt(myTransform.position.x), myTransform.position.y, Mathf.RoundToInt(myTransform.position.z));
        Instantiate(bomb);       
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
        }

        if (col.CompareTag("Flames"))
        {
            Flames++;
        }

        if (col.CompareTag("Speed"))
        {
            moveSpeed += 5f;
        }

        if (col.CompareTag("WallPass"))
        {
            Wallpass = true;
        }

        if (col.CompareTag("BombPass"))
        {
            Bombpass = true;
        }

        if (col.CompareTag("FlamePass"))
        {
            Flamepass = true;
        }

        if (col.CompareTag("Detonator"))
        {
            Detonator = true;
        }
    }

    //private void UpdatePlayerMovement()
    //{
    //    RaycastHit hit;
    //    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
    //    { //В верх          
    //        myTransform.rotation = new Quaternion(0, 0, 0, 0);
    //        target = new Vector3(myTransform.position.x, myTransform.position.y, myTransform.position.z + distlLenght);
    //        if (!Physics.Linecast(myTransform.position, target))
    //            rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
    //    }
    //    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
    //    { //В низ    
    //        myTransform.rotation = new Quaternion(0, 90, 0, 0);
    //        target = new Vector3(myTransform.position.x, myTransform.position.y, myTransform.position.z - distlLenght);
    //        if (!Physics.Linecast(myTransform.position, target))
    //            rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
    //    }
    //    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
    //    { //на право
    //        myTransform.rotation = new Quaternion(0, -90, 0, 90);
    //        target = new Vector3(myTransform.position.x - distlLenght, myTransform.position.y, myTransform.position.z);
    //        if (!Physics.Linecast(myTransform.position, target))
    //            rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
    //    }
    //    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
    //    { //на лево
    //        myTransform.rotation = new Quaternion(0, 90, 0, 90);
    //        target = new Vector3(myTransform.position.x + distlLenght, myTransform.position.y, myTransform.position.z);
    //        if (!Physics.Linecast(myTransform.position, target))
    //            rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
    //    }
    //}

}
