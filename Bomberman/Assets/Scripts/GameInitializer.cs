using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour {

    public GameObject floor;
    //public ObjectCreator floor;
    public GameObject walls;  
    public GameObject player;
    public GameObject enemy1;
    public GameObject enemy2;

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

        for (int i = 5; i < 12 * 5; i += 5)
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

        enemy1 = (Resources.Load("Models/Enemy1", typeof(GameObject))) as GameObject;
        enemy1.transform.position = new Vector3(-15f, 0, 15f);
        enemy1.transform.localScale = new Vector3(5f, 5f, 5f);
        Instantiate(enemy1);

        enemy2 = (Resources.Load("Models/Enemy2", typeof(GameObject))) as GameObject;
        enemy2.transform.position = new Vector3(-45f, 0, 45f);
        enemy2.transform.localScale = new Vector3(5f, 5f, 5f);
        Instantiate(enemy2);
    }
    void Update ()
    {

    }
}
