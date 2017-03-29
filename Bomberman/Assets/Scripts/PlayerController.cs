using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 2;

    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        { //В верх
            GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 0, 0, 0);
            GetComponent<Rigidbody>().transform.position += GetComponent<Rigidbody>().transform.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        { //В низ       
            GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 90, 0, 0);
            GetComponent<Rigidbody>().transform.position += GetComponent<Rigidbody>().transform.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        { //на право
            GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, -90, 0, 90);
            GetComponent<Rigidbody>().transform.position += GetComponent<Rigidbody>().transform.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        { //на лево
            GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 90, 0, 90);
            GetComponent<Rigidbody>().transform.position += GetComponent<Rigidbody>().transform.forward * speed * Time.deltaTime;

        }

        //if (Physics.Linecast(transform.position, target.position))
    }




}
