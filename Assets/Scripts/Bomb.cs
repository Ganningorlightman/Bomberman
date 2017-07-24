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
    public float BombSize = 5;
    private GameInitializer gameIni;

    Action callback;
    void Start()
    {       
        explosion = ObjectLoader.GetObject("Models/Explosion");
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
        gameIni = GameObject.FindObjectOfType<GameInitializer>();
        Vector3 old = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        old = Transformation(old);
        gameIni.Map.ChangeCellUnitType(old.x, old.z, UnitType.Empty);
        Destroy(gameObject, 0.5f);
        if (callback != null)
            callback();
    }
    private IEnumerator CreateExplosion(Vector3 direction)
    {
        for (int i = 1; i < Flames + 1; i++)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, direction, out hit, i * BombSize, wall);
            RaycastHit hit1;
            Physics.Raycast(transform.position, direction, out hit1, i * BombSize, wWall);

            if ((!hit1.collider) && (!hit.collider))
                Instantiate(explosion, transform.position + (i * direction * BombSize), explosion.transform.rotation);
            else if ((hit1.collider) && (!hit.collider))
            {              
                    Instantiate(explosion, transform.position + (i * direction * BombSize), explosion.transform.rotation);
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
    private Vector3 Transformation(Vector3 old) {
        return new Vector3(Mathf.CeilToInt(-old.x / GameInitializer.BlockSize), 0, Mathf.CeilToInt(old.z / GameInitializer.BlockSize));
    }
}
