using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public GameObject explosion;
    public LayerMask wall;
    public LayerMask wWall;
    public int Flames;
    public bool Detonator;

    Action callback;
    void Start()
    {       
        explosion = ObjectLoader.getObject("Models/Explosion");
    }
    public void Initialized(Action callback)
    {
        this.callback = callback;
    }

    void Update()
    {
        if (Detonator)
        {
            if (Input.GetKey(KeyCode.E)) Explode();
        }
        else
            Invoke("Explode", 3f);
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        StartCoroutine(CreateExplosion(Vector3.forward));
        StartCoroutine(CreateExplosion(Vector3.back));
        StartCoroutine(CreateExplosion(Vector3.right));
        StartCoroutine(CreateExplosion(Vector3.left));
        Destroy(gameObject);
        if (callback != null)
            callback();
    }
    private IEnumerator CreateExplosion(Vector3 direction)
    {
        for (int i = 1; i < Flames + 1; i++)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, direction, out hit, i * 5f, wall);
            RaycastHit hit1;
            Physics.Raycast(transform.position, direction, out hit1, i * 5f, wWall);

            if ((!hit1.collider) && (!hit.collider))
                Instantiate(explosion, transform.position + (i * direction * 5f), explosion.transform.rotation);
            else if ((hit1.collider) && (!hit.collider))
            {              
                    Instantiate(explosion, transform.position + (i * direction * 5f), explosion.transform.rotation);
                    break;                       
            }
            else break;

        }
        yield return new WaitForSeconds(.05f);
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Explosion"))
        {
            CancelInvoke("Explode");
            Explode();
        }
    }
}
