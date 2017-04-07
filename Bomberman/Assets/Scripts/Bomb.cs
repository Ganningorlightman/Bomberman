using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public GameObject explosion;
    public LayerMask wall;
    void Start()
    {
        explosion = (Resources.Load("Models/Explosion", typeof(GameObject))) as GameObject;
        Invoke("Explode", 3f);
    }

    void Update()
    {
       
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        StartCoroutine(CreateExplosion(Vector3.forward));
        StartCoroutine(CreateExplosion(Vector3.back));
        StartCoroutine(CreateExplosion(Vector3.right));
        StartCoroutine(CreateExplosion(Vector3.left));
    }
    private IEnumerator CreateExplosion(Vector3 direction)
    {
        for (int i = 1; i < 3; i++)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, direction, out hit, i * 5f, wall);

            if (!hit.collider)           
                Instantiate(explosion, transform.position + (i * direction * 5f), explosion.transform.rotation);       
            else            
                break;                       
        }
        yield return new WaitForSeconds(.05f);
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Explosion"))
        {
            Destroy(gameObject);
        }
    }
}
