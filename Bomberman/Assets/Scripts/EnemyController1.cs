﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController1 : MonoBehaviour {
    	
	public float speed = 5f;
    private Vector3 target;
    private float myTime = 0f;
    private int direc;
    private CharacterController charContr;
    public LayerMask wallLayer;
    public LayerMask wWallLayer;
    public LayerMask bombLayer;
    public bool Wallpass;
    public int points;

    void Start()
    {
        charContr = GetComponent<CharacterController>();
    }
    void Update()
    {                       
        if (myTime <= 0)
        {
            direc = Random.Range(1, 5);
            myTime = 1.5f;          
        }
        else myTime -= Time.deltaTime;

        switch (direc)
        {
            case 1:
                { //В верх
                    GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 0, 0, 0);
                    if (CheckLayers())
                        transform.position += transform.forward * speed * Time.deltaTime;
                    else myTime = 0;
                    break;
                }
            case 2:
                { //В низ       
                    GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 90, 0, 0);
                    if (CheckLayers())
                        transform.position += transform.forward * speed * Time.deltaTime;
                    else myTime = 0;
                    break;
                }
            case 3:
                { //на право
                    GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, -90, 0, 90);
                    if (CheckLayers())
                        transform.position += transform.forward * speed * Time.deltaTime;
                    else myTime = 0;
                    break;
                }
            case 4:
                { //на лево
                    GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 90, 0, 90);
                    if (CheckLayers())
                        transform.position += transform.forward * speed * Time.deltaTime;
                    else myTime = 0;
                    break;
                }
        }

        //switch (direc)
        //    {
        //        case 1:
        //            { //В верх
        //                GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 0, 0, 0);
        //                target = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.5f);
        //            if (!Physics.Linecast(transform.position, target))
        //                transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        //            else myTime = 0;
        //            break;
        //            }
        //        case 2:
        //            { //В низ       
        //                GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 90, 0, 0);
        //                target = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1.5f);
        //                if (!Physics.Linecast(transform.position, target))
        //                    transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        //            else myTime = 0;
        //            break;
        //            }
        //        case 3:
        //            { //на право
        //                GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, -90, 0, 90);
        //                target = new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z);
        //                if (!Physics.Linecast(transform.position, target))
        //                    transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        //            else myTime = 0;
        //            break;
        //            }
        //        case 4:
        //            { //на лево
        //                GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, 90, 0, 90);
        //                target = new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z);
        //                if (!Physics.Linecast(transform.position, target))
        //                    transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        //            else myTime = 0;
        //            break;
        //            }
        //    }
    }

    private bool CheckLayers()
    {
        RaycastHit hit;
        Vector3 p1 = transform.position + charContr.center;

        if ((!Physics.SphereCast(p1, charContr.height / 2, transform.forward, out hit, 2.5f, wallLayer)) && (!Physics.SphereCast(p1, charContr.height / 2, transform.forward, out hit, 2.5f, bombLayer)) && Wallpass)
        {
            return true;
        }
        else if ((!Physics.SphereCast(p1, charContr.height / 2, transform.forward, out hit, 2.5f, wallLayer)) && (!Physics.SphereCast(p1, charContr.height / 2, transform.forward, out hit, 2.5f, bombLayer)) && (!Physics.SphereCast(p1, charContr.height / 2, transform.forward, out hit, 2.5f, wWallLayer)) && !Wallpass)
        {
            return true;
        }
        else
        {
            return false;
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
