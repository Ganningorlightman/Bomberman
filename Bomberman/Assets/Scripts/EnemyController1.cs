using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController1 : MonoBehaviour {

    public float speed = 5f;
    private Vector3 target;
    private float myTime = 0f;
    private int direc;
    private CharacterController charContr;
    public LayerMask wall;

    void Start()
    {
        charContr = GetComponent<CharacterController>();
    }
    void Update()
    {
        RaycastHit hit;
        Vector3 p1 = transform.position + charContr.center;

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
                    if (!Physics.SphereCast(p1, charContr.height / 2, transform.forward, out hit, 2.5f, wall))
                        transform.position += transform.forward * speed * Time.deltaTime;
                    else myTime = 0;
                    break;
                }
            case 2:
                { //В низ       
                    GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 90, 0, 0);
                    if (!Physics.SphereCast(p1, charContr.height / 2, transform.forward, out hit, 2.5f, wall))
                        transform.position += transform.forward * speed * Time.deltaTime;
                    else myTime = 0;
                    break;
                }
            case 3:
                { //на право
                    GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, -90, 0, 90);
                    if (!Physics.SphereCast(p1, charContr.height / 2, transform.forward, out hit, 2.5f, wall))
                        transform.position += transform.forward * speed * Time.deltaTime;
                    else myTime = 0;
                    break;
                }
            case 4:
                { //на лево
                    GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 90, 0, 90);
                    if (!Physics.SphereCast(p1, charContr.height / 2, transform.forward, out hit, 2.5f, wall))
                        transform.position += transform.forward * speed * Time.deltaTime;
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