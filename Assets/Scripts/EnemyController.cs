using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private float myTime = 0f;
    private int direc;
    private Rigidbody rigidBody;
    private CharacterController charContr;

    public LayerMask wallLayer;
    public LayerMask wWallLayer;
    public LayerMask bombLayer;
    private PlayerController playerContr;
    private GameInitializer gameIni;

    public float speed;
    public bool Wallpass;
    public int points;
    public int Smart;
    private bool dead = false;
    private Node nextStep;
    private Vector3 lastPosition;

    void Start() {
        playerContr = GameObject.FindObjectOfType<PlayerController>();
        gameIni = GameObject.FindObjectOfType<GameInitializer>();
        rigidBody = GetComponent<Rigidbody>();
        charContr = GetComponent<CharacterController>();
        rigidBody.transform.rotation = Quaternion.Euler(0, 180, 0);
        lastPosition = transform.position;
    }
    void Update() {
        if(!dead && !playerContr.Death) {
            switch(Smart) {
                case 1: {
                        RandomDirection();
                        break;
                    }
                case 2: {
                        Search();
                        break;
                    }
            }
        }
    }
    private void Search() {
        Vector3 temp = new Vector3();
        Vector3 start = new Vector3();
        Vector3 end = new Vector3();
        var pathFinder = new PathFinder(gameIni.Map, Wallpass);
        start = Transformation(new Vector3(-Rounding(transform.position.x, lastPosition.x), 0, Rounding(transform.position.z, lastPosition.z)));
        end = Transformation(RoundingEnd(playerContr.PlayerPosition.x, playerContr.PlayerPosition.z));
        var path = pathFinder.FindPath(start, end);
        if(path.Any()) {
            nextStep = path.Pop();
            temp = (Transformation(start) - Transformation(new Vector3(nextStep.X, 0f, nextStep.Z)));
            if((temp.x == 0) && (temp.z == 0)) {
                nextStep = path.Pop();
                temp = (Transformation(start) - Transformation(new Vector3(nextStep.X, 0f, nextStep.Z)));
            }
            if(temp.z < 0) { //Вверх              
                rigidBody.transform.rotation = Quaternion.Euler(0, 0, 0);
            } else if(temp.z > 0) { //Вниз       
                rigidBody.transform.rotation = Quaternion.Euler(0, 180, 0);
            } else if(temp.x > 0) { //направо
                rigidBody.transform.rotation = Quaternion.Euler(0, 270, 0);
            } else if(temp.x < 0) { //налево
                rigidBody.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            Move();
        }
    }
    private Vector3 Transformation(Vector3 old) {
        if(old.x < 0) {
            return new Vector3(Mathf.CeilToInt(-old.x / GameInitializer.BlockSize), 0, Mathf.CeilToInt(old.z / GameInitializer.BlockSize));
        } else return (new Vector3(-Mathf.CeilToInt(old.x * GameInitializer.BlockSize), 0, Mathf.CeilToInt(old.z * GameInitializer.BlockSize)));
    }
    private int Rounding(float x, float lastX) {
        var Size = GameInitializer.BlockSize;
        x = Mathf.Abs(x);
        lastX = Mathf.Abs(lastX);
        float xi = x % Size;
        if(xi == Size) return (int)x;
        if(lastX < x) {
            xi = Size - xi;
            if(xi <= 0.1f)
                x = x - (x % Size) + Size;
            else x = x - (x % Size);
        } else if(lastX > x) {
            if(xi >= 0.1f)
                x = x - (x % Size) + Size;
            else x = x - (x % Size);
        }
        return (int)x;
    }
    private Vector3 RoundingEnd(float x, float z) {
        x = Mathf.RoundToInt(x);
        z = Mathf.RoundToInt(z);
        var Size = GameInitializer.BlockSize;
        if((Mathf.Abs(x) % Size) >= (int)(Size / 2))
            x = x - (x % Size) - Size;
        else x = x - (x % Size);
        if((Mathf.Abs(z) % Size) >= (int)(Size / 2))
            z = z - (z % Size) + Size;
        else z = z - (z % Size);
        return new Vector3(x, 0f, z);
    }

    private void RandomDirection() {
        if(myTime <= 0) {
            direc = Random.Range(1, 5);
            myTime = 1.5f;
        } else myTime -= Time.deltaTime;

        switch(direc) {
            case 1: { //Вверх
                    rigidBody.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                }
            case 2: { //Вниз       
                    rigidBody.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                }
            case 3: { //направо
                    rigidBody.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                }
            case 4: { //налево
                    rigidBody.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                }
        }
        Move();
    }
    private void Move() {
        if(CheckLayers(transform.position)) {
            lastPosition = transform.position;
            transform.position += transform.forward * speed * Time.deltaTime;
        } else myTime = 0;
    }
    private bool CheckLayers(Vector3 position) {
        RaycastHit hit;
        Vector3 p1 = position + charContr.center;

        if((!Physics.SphereCast(p1, charContr.height, transform.forward, out hit, 1.5f, wallLayer)) && (!Physics.SphereCast(p1, charContr.height, transform.forward, out hit, 1.5f, bombLayer)) && Wallpass) {
            return true;
        } else if((!Physics.SphereCast(p1, charContr.height, transform.forward, out hit, 1.5f, wallLayer)) && (!Physics.SphereCast(p1, charContr.height, transform.forward, out hit, 1.5f, bombLayer)) && (!Physics.SphereCast(p1, charContr.height, transform.forward, out hit, 1.5f, wWallLayer)) && !Wallpass) {
            return true;
        } else {
            return false;
        }
    }
    public void OnTriggerEnter(Collider col) {
        if(col.CompareTag("Explosion") && (!dead)) {
            dead = true;
            GetComponent<Collider>().enabled = false;
            GameController.Enemy--;
            GameController.Score += points;
            Destroy(gameObject, 2f);
        }
    }
}
