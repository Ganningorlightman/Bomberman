using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour {

    public GameObject floor;
    //public ObjectCreator floor;
    public GameObject walls;

    public GameObject player;
    //public float speed = 5f;
    //public float rotationSpeed = 1200f;
    //private bool walk;
    //private bool _run;
    //// Анимации
    //public AnimationClip a_Idle;
    //public float a_IdleSpeed = 1;
    //public AnimationClip a_Walk;
    //public float a_WalkSpeed = 1;
    //public AnimationClip a_Run;
    //public float a_RunSpeed = 2;


    void Start ()
    {
        //floor.Create(new Vector3(6.5f, 1f, 5.5f), "Models/Floor", "Floor");
        //floor.InstantiateObject(new Vector3(-30f, -2.5f, 25f));
        floor = (Resources.Load("Models/Floor", typeof(GameObject))) as GameObject;
        floor.transform.position = new Vector3(-30f, -2.5f, 25f);
        floor.transform.localScale = new Vector3(6.5f, 1f, 5.5f);
        Instantiate(floor);

        walls = (Resources.Load("Models/Wall", typeof(GameObject))) as GameObject;
        walls.transform.localScale = new Vector3(5f, 5f, 5f);

        for (int i = 0; i < 11 * 5; i += 5)
        {
            walls.transform.position = new Vector3(0, 0, i);
            Instantiate(walls);
            walls.transform.position = new Vector3(-12 * 5, 0, i);
            Instantiate(walls);
        }

        for (int i = 0; i < 13 * 5; i += 5)
        {
            walls.transform.position = new Vector3(-i, 0, 0);
            Instantiate(walls);
            walls.transform.position = new Vector3(-i, 0, 10 * 5);
            Instantiate(walls);
        }

        for (int i = 5; i < 10 * 5; i += 5)
            for(int j = 5; j < 12 * 5; j += 5)
        {
                if ((j % 2 == 0) && (i % 2 == 0))
                {
                    walls.transform.position = new Vector3(-j, 0, i);
                    Instantiate(walls);
                }
        }

        player = (Resources.Load("Models/Player", typeof(GameObject))) as GameObject;
        player.transform.position = new Vector3(-5f, 0, 5f);
        player.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        Instantiate(player);

        //animation[a_Idle.name].speed = a_IdleSpeed;
        //animation[a_Walk.name].speed = a_WalkSpeed; // шаг
        //animation[a_Run.name].speed = a_RunSpeed; //бег
        //animation.CrossFade(a_Idle.name); //анимайия стартового состояния (покоя)
        //_run = false;

    }
    void Update ()
    {
        //    //Переход бег/ходьба
        //    if (Input.GetKeyUp("left shift"))
        //    {
        //        if (_run == false)
        //        {
        //            speed = speed + 5;
        //            rotationSpeed = rotationSpeed + 5;
        //            _run = !_run;
        //        }
        //        else
        //        {
        //            speed = speed - 5;
        //            rotationSpeed = rotationSpeed - 5;
        //            _run = !_run;
        //        }
        //    }
        //    //Движение с клавиатуры
        //    // Передвижение             
        //    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        //    { //В верх
        //        player.transform.position += player.transform.forward * speed * Time.deltaTime;

        //        if (_run)
        //        {
        //            animation.CrossFade(a_Run.name);
        //        }
        //        else
        //        {
        //            animation.CrossFade(a_Walk.name);
        //        }
        //    }
        //    else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        //    { //В низ
        //        player.transform.position -= player.transform.forward * speed * Time.deltaTime;

        //        if (_run)
        //        {
        //            animation.CrossFade(a_Run.name);
        //        }
        //        else
        //        {
        //            animation.CrossFade(a_Walk.name);
        //        }
        //    }
        //    else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //    { //на право
        //        player.transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);

        //        if (_run)
        //        {
        //            animation.CrossFade(a_Run.name);
        //        }
        //        else
        //        {
        //            animation.CrossFade(a_Walk.name);
        //        }
        //    }
        //    else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //    { //на лево

        //        //transform.Rotate(MyMousLook.positionCamera * rotationSpeed * Time.deltaTime);
        //        //transform.rotation = Quaternion.identity;
        //        if (_run)
        //        {
        //            animation.CrossFade(a_Run.name);
        //        }
        //        else
        //        {
        //            animation.CrossFade(a_Walk.name);
        //        }

        //        //transform.position = Vector3(transform.position.x + 1, transform.position.y, ransform.position.z);
        //    }
        //    else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)
        //            || (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        //            || (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        //            || (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)))
        //        animation.CrossFade(a_Idle.name);

    }
}
