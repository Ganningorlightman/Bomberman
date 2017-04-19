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
                        rigidBody.transform.rotation = new Quaternion(0, 0, 0, 0);
                        break;
                    }
                case 2:
                    { //В низ       
                        rigidBody.transform.rotation = new Quaternion(0, 90, 0, 0);
                        break;
                    }
                case 3:
                    { //на право
                        rigidBody.transform.rotation = new Quaternion(0, -90, 0, 90);
                        break;
                    }
                case 4:
                    { //на лево
                        rigidBody.transform.rotation = new Quaternion(0, 90, 0, 90);
                        break;
                    }
            }
            Move();
        }
    }

    private void Move()
    {
        if (CheckLayers())
            transform.position += transform.forward * speed * Time.deltaTime;
        else myTime = 0;
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
        if (col.CompareTag("Explosion") && (!dead))
        {
            dead = true;
            Destroy(gameObject, 2f);
            GetComponent<Collider>().enabled = false;
            GameController.Enemy--;
            GameController.Points += points;
            if (GameController.Enemy == 0) GameController.ExitOpen = true;
        }
    }
}
