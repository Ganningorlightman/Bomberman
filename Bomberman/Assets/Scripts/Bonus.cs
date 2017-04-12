using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{

    public void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
