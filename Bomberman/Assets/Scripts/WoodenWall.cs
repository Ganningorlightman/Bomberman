using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenWall : MonoBehaviour {

    public bool test = false;
    
    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Explosion"))
        {
            Destroy(gameObject);
        }
    }

}
