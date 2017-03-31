using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    	
	public float speed = 2;
    Vector3 target;
    void Start()
    {
   
    }
    void Update()
    {
        { //В верх
            GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 0, 0, 0);
            target = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.5f);
            if (!Physics.Linecast(transform.position, target))
                GetComponent<Rigidbody>().transform.position += GetComponent<Rigidbody>().transform.forward * speed * Time.deltaTime;
        }

        { //В низ       
            GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 90, 0, 0);
            target = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1.5f);
            if (!Physics.Linecast(transform.position, target))
                GetComponent<Rigidbody>().transform.position += GetComponent<Rigidbody>().transform.forward * speed * Time.deltaTime;
        }

        { //на право
            GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, -90, 0, 90);
            target = new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z);
            if (!Physics.Linecast(transform.position, target))
                GetComponent<Rigidbody>().transform.position += GetComponent<Rigidbody>().transform.forward * speed * Time.deltaTime;
        }

        { //на лево
            GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 90, 0, 90);
            target = new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z);
            if (!Physics.Linecast(transform.position, target))
                GetComponent<Rigidbody>().transform.position += GetComponent<Rigidbody>().transform.forward * speed * Time.deltaTime;
        }

        //Coroutine

    }
}
