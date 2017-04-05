using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public GameObject explosion;

    void Start()
    {
        //explosion = (Resources.Load("Models/Explosion", typeof(GameObject))) as GameObject;
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
        GetComponent<MeshRenderer>().enabled = false;
        transform.FindChild("Collider").gameObject.SetActive(false);
        Destroy(gameObject, 3f);
    }
    private IEnumerator CreateExplosion(Vector3 direction)
    {

        for (int i = 1; i < 3; i++)
        {
            RaycastHit hit;

            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit, i, 8);

            if (!hit.collider)
            {
                Instantiate(explosion, transform.position + (i * direction),
                  explosion.transform.rotation);
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(.05f);
        }
    }
}
