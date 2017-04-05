using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    	
	public float speed = 5f;
    private Vector3 target;
    private float myTime = 1f;
    private int direc;

    void Start()
    {
   
    }
    void Update()
    {      
        if (myTime <= 0)
        {
            direc = Random.Range(1, 5);
            myTime = 1f;          
        }
        else myTime -= Time.deltaTime;

        switch (direc)
            {
                case 1:
                    { //В верх
                        GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 0, 0, 0);
                        target = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.5f);
                    if (!Physics.Linecast(transform.position, target))
                        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
                    else myTime = 0;
                    break;
                    }
                case 2:
                    { //В низ       
                        GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 90, 0, 0);
                        target = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1.5f);
                        if (!Physics.Linecast(transform.position, target))
                            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
                    else myTime = 0;
                    break;
                    }
                case 3:
                    { //на право
                        GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, -90, 0, 90);
                        target = new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z);
                        if (!Physics.Linecast(transform.position, target))
                            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
                    else myTime = 0;
                    break;
                    }
                case 4:
                    { //на лево
                        GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 90, 0, 90);
                        target = new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z);
                        if (!Physics.Linecast(transform.position, target))
                            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
                    else myTime = 0;
                    break;
                    }
            }

    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Explosion"))
        {
            Destroy(gameObject);
        }
    }
}
