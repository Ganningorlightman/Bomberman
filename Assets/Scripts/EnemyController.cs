using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    		
    private float myTime = 0f;
    private int direc;
    private Rigidbody rigidBody;
    private CharacterController charContr;

    public LayerMask wallLayer;
    public LayerMask wWallLayer;
    public LayerMask bombLayer;

    public float speed;
    public bool Wallpass;
    public int points;
    public int Smart;
    private bool dead = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        charContr = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (!dead)
        {
            RandomDirection();
        }
    }

    private void RandomDirection() {
        if(myTime <= 0) {
            direc = Random.Range(1, 5);
            myTime = 1.5f;
        } else myTime -= Time.deltaTime;

        switch(direc) {
            case 1: { //В верх
                    rigidBody.transform.rotation = new Quaternion(0, 0, 0, 0);
                    break;
                }
            case 2: { //В низ       
                    rigidBody.transform.rotation = new Quaternion(0, 90, 0, 0);
                    break;
                }
            case 3: { //на право
                    rigidBody.transform.rotation = new Quaternion(0, -90, 0, 90);
                    break;
                }
            case 4: { //на лево
                    rigidBody.transform.rotation = new Quaternion(0, 90, 0, 90);
                    break;
                }
        }
        Move();
    }
    private void Move()
    {
        if (CheckLayers(transform.position))
            transform.position += transform.forward * speed * Time.deltaTime;
        else myTime = 0;
    }
    private bool CheckLayers(Vector3 position)
    {
        RaycastHit hit;
        Vector3 p1 = position + charContr.center;

        if ((!Physics.SphereCast(p1, charContr.height, transform.forward, out hit, 1.5f, wallLayer)) && (!Physics.SphereCast(p1, charContr.height, transform.forward, out hit, 1.5f, bombLayer)) && Wallpass)
        {
            return true;
        }
        else if ((!Physics.SphereCast(p1, charContr.height, transform.forward, out hit, 1.5f, wallLayer)) && (!Physics.SphereCast(p1, charContr.height, transform.forward, out hit, 1.5f, bombLayer)) && (!Physics.SphereCast(p1, charContr.height, transform.forward, out hit, 1.5f, wWallLayer)) && !Wallpass)
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
        if (col.CompareTag("Explosion") && (!dead))
        {
            dead = true;
            Destroy(gameObject, 2f);
            GetComponent<Collider>().enabled = false;
            GameController.Enemy--;
            GameController.Score += points;
        }
    }
}
