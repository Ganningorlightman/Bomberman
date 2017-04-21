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
    private bool destroy = false;
    public AudioSource ExpAudio;

    Action callback;
    void Start()
    {       
        explosion = ObjectLoader.getObject("Models/Explosion");
        if (!Detonator) Invoke("Explode", 3f);
    }
    public void Initialized(Action callback)
    {
        this.callback = callback;
    }

    void Update()
    {        
       if (Input.GetKey(KeyCode.E) && (Detonator) && (!destroy))            
                Explode();                                                                      
    }

    void Explode()
    {
        destroy = true;
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        Instantiate(explosion, transform.position, Quaternion.identity);
        StartCoroutine(CreateExplosion(Vector3.forward));
        StartCoroutine(CreateExplosion(Vector3.back));
        StartCoroutine(CreateExplosion(Vector3.right));
        StartCoroutine(CreateExplosion(Vector3.left));
        ExpAudio.Play();
        Destroy(gameObject, 0.5f);
        if (callback != null)
            callback();
    }
    private IEnumerator CreateExplosion(Vector3 direction)
    {
        for (int i = 1; i < Flames + 1; i++)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, direction, out hit, i * GameController.BlockAndUnitsSize, wall);
            RaycastHit hit1;
            Physics.Raycast(transform.position, direction, out hit1, i * GameController.BlockAndUnitsSize, wWall);

            if ((!hit1.collider) && (!hit.collider))
                Instantiate(explosion, transform.position + (i * direction * GameController.BlockAndUnitsSize), explosion.transform.rotation);
            else if ((hit1.collider) && (!hit.collider))
            {              
                    Instantiate(explosion, transform.position + (i * direction * GameController.BlockAndUnitsSize), explosion.transform.rotation);
                    break;                       
            }
            else break;

        }
        yield return new WaitForSeconds(.05f);
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Explosion") && (!destroy))
        {
            CancelInvoke("Explode");
            Explode();
        }
    }
}
