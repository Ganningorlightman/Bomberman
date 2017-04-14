using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour {

    public GameObject floor;
    public GameObject walls;
    public GameObject wWalls;
    public GameObject player;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject bonus;

    void Start ()
    {
        floor = ObjectLoader.getObject("Models/Floor");
        floor.transform.localScale = new Vector3(6.5f, 1f, 5.5f);
        floor.transform.position = new Vector3(-30f, -2.5f, 25f);   
        Instantiate(floor);

        walls = ObjectLoader.getObject("Models/Wall");       

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

        wWalls = ObjectLoader.getObject("Models/WWall");
        wWalls.transform.position = new Vector3(-15f, 0, 5f);           
        Instantiate(wWalls);
        wWalls.transform.position = new Vector3(-20f, 0, 5f);
        Instantiate(wWalls);
        wWalls.transform.position = new Vector3(-10f, 0, 15f);
        Instantiate(wWalls);
        wWalls.transform.position = new Vector3(-35f, 0, 15f);
        Instantiate(wWalls);
        wWalls.transform.position = new Vector3(-35f, 0, 20f);
        Instantiate(wWalls);
        wWalls.transform.position = new Vector3(-30f, 0, 15f);
        Instantiate(wWalls);
        wWalls.transform.position = new Vector3(-40f, 0, 15f);
        Instantiate(wWalls);
        wWalls.transform.position = new Vector3(-35f, 0, 10f);
        Instantiate(wWalls);
        wWalls.transform.position = new Vector3(-55f, 0, 10f);
        Instantiate(wWalls);
        wWalls.transform.position = new Vector3(-55f, 0, 5f);
        Instantiate(wWalls);
        wWalls.transform.position = new Vector3(-50f, 0, 5f);
        Instantiate(wWalls);
        wWalls.transform.position = new Vector3(-25f, 0, 30f);
        Instantiate(wWalls);
        wWalls.transform.position = new Vector3(-25f, 0, 35f);
        Instantiate(wWalls);
        wWalls.transform.position = new Vector3(-25f, 0, 40f);
        Instantiate(wWalls);      

        bonus = ObjectLoader.getObject("Models/Bombs");
        Instantiate(bonus);
        bonus = ObjectLoader.getObject("Models/Flames");
        Instantiate(bonus);
        bonus = ObjectLoader.getObject("Models/Speed");
        Instantiate(bonus);
        bonus = ObjectLoader.getObject("Models/WallPass");
        Instantiate(bonus);
        bonus = ObjectLoader.getObject("Models/BombPass");
        Instantiate(bonus);
        bonus = ObjectLoader.getObject("Models/FlamePass");
        Instantiate(bonus);
        bonus = ObjectLoader.getObject("Models/Detonator");
        Instantiate(bonus);

        player = ObjectLoader.getObject("Models/Player1");
        Instantiate(player);

        enemy1 = ObjectLoader.getObject("Models/Enemy1");
        enemy1.transform.position = new Vector3(-15f, 0, 15f);     
        Instantiate(enemy1);
        GameController.Enemy++;
        enemy1.transform.position = new Vector3(-35f, 0, 5f);
        Instantiate(enemy1);
        GameController.Enemy++;

        enemy2 = ObjectLoader.getObject("Models/Enemy2");
        enemy2.transform.position = new Vector3(-45f, 0, 45f);     
        Instantiate(enemy2);
        GameController.Enemy++;
    }
    void Update ()
    {

    }
}
