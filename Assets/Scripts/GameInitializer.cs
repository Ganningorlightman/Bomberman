using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour {

    public GameObject floor;
    public GameObject wall;
    public GameObject wWall;
    public GameObject player;
    public GameObject enemy1;
    public GameObject enemy2;
    //int blockSize = 5;
    //public int BlockSize {
    //    get { return blockSize; }
    //    set {
    //        floor.gameObject.transform.localScale = new Vector3(value, value, value);
    //        wall.gameObject.transform.localScale = new Vector3(value, value, value);
    //        wWall.gameObject.transform.localScale = new Vector3(value, value, value);
    //        enemy1.gameObject.transform.localScale = new Vector3(value, value, value);
    //        enemy2.gameObject.transform.localScale = new Vector3(value, value, value);
    //        blockSize = value;
    //    }
    //}
    public int BlockSize = 5;
    public int MapWidth = 9;
    public int MapHeight = 6;

    void Start() {
        GenerateMap(MapWidth, MapHeight);
        GeneratePlayerAndEnemy();
        GenerateWWall(MapWidth, MapHeight);
    }

    void OnGUI() {
        GUI.Label(new Rect(10, 10, 100, 100), "Score: " + GameController.Score);
    }

    void GenerateMap(int width, int height) {

        floor = ObjectLoader.GetObject("Models/Floor");
        floor.transform.localScale = new Vector3(width + 1.5f, 1f, height + 1.5f);
        floor.transform.position = new Vector3(-(width + 1f) * BlockSize, -(BlockSize / 2f), (height + 1) * BlockSize);
        

        wall = ObjectLoader.GetObject("Models/Wall");
        for(int i = 0; i < (height * 2 + 3) * BlockSize; i += BlockSize) {
            wall.transform.position = new Vector3(0, 0, i);
            Instantiate(wall);
            wall.transform.position = new Vector3(-(width * 2f + 2f) * BlockSize, 0, i);
            Instantiate(wall);
        }
        for(int i = BlockSize; i < (width * 2 + 2) * BlockSize; i += BlockSize) {
            wall.transform.position = new Vector3(-i, 0, 0);
            Instantiate(wall);
            wall.transform.position = new Vector3(-i, 0, (height * 2f + 2f) * BlockSize);
            Instantiate(wall);
        }
        for(int i = BlockSize; i < (height * 2 + 3) * BlockSize; i += BlockSize)
            for(int j = BlockSize; j < (width * 2 + 3) * BlockSize; j += BlockSize) {
                if((j % 2 == 0) && (i % 2 == 0)) {
                    wall.transform.position = new Vector3(-j, 0, i);
                    Instantiate(wall);
                }
            }

        Instantiate(floor);
    }

    void GeneratePlayerAndEnemy() {
        player = ObjectLoader.GetObject("Models/Player1");
        Instantiate(player);
        enemy1 = ObjectLoader.GetObject("Models/Enemy1");
        enemy1.transform.position = new Vector3(-15f, 0, 15f);
        Instantiate(enemy1);
        GameController.Enemy++;
        enemy1.transform.position = new Vector3(-35f, 0, 5f);
        Instantiate(enemy1);
        GameController.Enemy++;
        enemy2 = ObjectLoader.GetObject("Models/Enemy2");
        enemy2.transform.position = new Vector3(-45f, 0, 45f);
        Instantiate(enemy2);
        GameController.Enemy++;
    }

    void GenerateWWall(int width, int height) {
        wWall = ObjectLoader.GetObject("Models/WWall");
        for(int i = BlockSize; i < (width * 2 + 2) * BlockSize; i += BlockSize)
            for(int j = BlockSize; j < (height * 2 + 2) * BlockSize; j += BlockSize) {
                if(Random.Range(0f, 1f) <= 0.25f) {
                    wWall.transform.position = new Vector3(-i, 0, j);
                    Instantiate(wWall);
                }
            }
    }
}
