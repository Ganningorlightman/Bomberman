﻿using System.Collections;
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
        floor = (Resources.Load("Models/Floor", typeof(GameObject))) as GameObject;
        floor.transform.localScale = new Vector3(6.5f, 1f, 5.5f);
        floor.transform.position = new Vector3(-30f, -2.5f, 25f);   
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

        wWalls = (Resources.Load("Models/WWall", typeof(GameObject))) as GameObject;
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

        bonus = (Resources.Load("Models/Bombs", typeof(GameObject))) as GameObject;      
        Instantiate(bonus);
        bonus = (Resources.Load("Models/Flames", typeof(GameObject))) as GameObject;
        Instantiate(bonus);
        bonus = (Resources.Load("Models/Speed", typeof(GameObject))) as GameObject;
        Instantiate(bonus);
        bonus = (Resources.Load("Models/WallPass", typeof(GameObject))) as GameObject;
        Instantiate(bonus);
        bonus = (Resources.Load("Models/BombPass", typeof(GameObject))) as GameObject;
        Instantiate(bonus);
        bonus = (Resources.Load("Models/FlamePass", typeof(GameObject))) as GameObject;
        Instantiate(bonus);
        bonus = (Resources.Load("Models/Detonator", typeof(GameObject))) as GameObject;
        Instantiate(bonus);

        player = (Resources.Load("Models/Player", typeof(GameObject))) as GameObject;
        player.transform.position = new Vector3(-5f, 0, 5f);    
        Instantiate(player);

        enemy1 = (Resources.Load("Models/Enemy1", typeof(GameObject))) as GameObject;
        enemy1.transform.position = new Vector3(-15f, 0, 15f);     
        Instantiate(enemy1);
        GameController.Enemy++;
        enemy1.transform.position = new Vector3(-35f, 0, 5f);
        Instantiate(enemy1);
        GameController.Enemy++;

        enemy2 = (Resources.Load("Models/Enemy2", typeof(GameObject))) as GameObject;
        enemy2.transform.position = new Vector3(-45f, 0, 45f);     
        Instantiate(enemy2);
        GameController.Enemy++;
    }
    void Update ()
    {

    }
}
