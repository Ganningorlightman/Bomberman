using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 15f;

    private Rigidbody rigidBody;
    private Transform myTransform;
    private Vector3 target;
    public GameObject bomb;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;  
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
        RaycastHit hit;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        { //В верх          
            myTransform.rotation = new Quaternion(0, 0, 0, 0);
            target = new Vector3(myTransform.position.x, myTransform.position.y, myTransform.position.z + 1.5f);
            if (!Physics.CapsuleCast(new Vector3(myTransform.position.x, myTransform.position.y - 1.25f, myTransform.position.z), new Vector3(myTransform.position.x, myTransform.position.y + 1.25f, myTransform.position.z), 2.5f, transform.forward, out hit, 0.1f))
                rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        { //В низ    
            myTransform.rotation = new Quaternion(0, 90, 0, 0);
            target = new Vector3(myTransform.position.x, myTransform.position.y, myTransform.position.z - 1.5f);
            if (!Physics.Linecast(myTransform.position, target))
                rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        { //на право
            myTransform.rotation = new Quaternion(0, -90, 0, 90);
            target = new Vector3(myTransform.position.x - 1.5f, myTransform.position.y, myTransform.position.z);
            if (!Physics.Linecast(myTransform.position, target))
                rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        { //на лево
            myTransform.rotation = new Quaternion(0, 90, 0, 90);
            target = new Vector3(myTransform.position.x + 1.5f, myTransform.position.y, myTransform.position.z);
            if (!Physics.Linecast(myTransform.position, target))
                rigidBody.transform.position += rigidBody.transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    private void DropBomb()
    {
        bomb = (Resources.Load("Models/Bomb", typeof(GameObject))) as GameObject;
        bomb.transform.position = new Vector3(Mathf.RoundToInt(myTransform.position.x), myTransform.position.y, Mathf.RoundToInt(myTransform.position.z));
        bomb.transform.localScale = new Vector3(5f, 5f, 5f);
            Instantiate(bomb);       
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy") || col.CompareTag("Explosion"))
        {
            Destroy(gameObject);
        }
    }

}
