using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenWall : MonoBehaviour {

    public bool exit;
    public bool bonus;
    private GameObject UnderObject;

    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Explosion"))
        {
            Destroy(gameObject);
            if (exit)
            {

                UnderObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }
            else if (bonus)
            {

                UnderObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }
        }
    }

}
