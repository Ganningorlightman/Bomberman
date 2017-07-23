using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
public class GameInitializer : MonoBehaviour {

    public static Map Map;

    public GameObject floor;
    public GameObject wall;
    public GameObject wWall;
    public GameObject player;
    public GameObject enemy1;
    public GameObject enemy2;

    public static int BlockSize = 5;
    public int MapWidth = 7;
    public int MapHeight = 7;

    void Start() {
        Instantiate(ObjectLoader.GetObject("Models/Directional Light"));
        GenerateMap(MapWidth, MapHeight, 3);
    }
    void OnGUI() {
        GUI.Label(new Rect(10, 10, 100, 100), "Score: " + GameController.Score);
    }
    void GenerateMap(int width, int height, int enemyCount) {
        if((width & 1) == 0 || (height & 1) == 0) {
            throw new InvalidOperationException("Width or Height must be odd");
        }
        floor = ObjectLoader.GetObject("Models/Floor");
        wall = ObjectLoader.GetObject("Models/Wall");
        wWall = ObjectLoader.GetObject("Models/WWall");
        player = ObjectLoader.GetObject("Models/Player1");
        enemy1 = ObjectLoader.GetObject("Models/Enemy1");
        enemy2 = ObjectLoader.GetObject("Models/Enemy2");

        floor.transform.localScale = new Vector3((width / 2) + 1.5f, 1f, (height / 2) + 1.5f);
        floor.transform.position = new Vector3(-(width + 1f) / 2, -0.5f, (height + 1f) / 2) * BlockSize;
        Instantiate(floor);

        Map = new Map(width, height);
        Map.ChangeCellUnitType(1, 1, UnitType.Stub);
        Map.ChangeCellUnitType(1, 2, UnitType.Stub);
        Map.ChangeCellUnitType(2, 1, UnitType.Stub);
        // map boder
        for(int i = 1; i < width + 1; i++) {
            Map.ChangeCellUnitType(i, 0, UnitType.Wall);
            Map.ChangeCellUnitType(i, height + 1, UnitType.Wall);
        }
        for(int i = 0; i < height + 2; i++) {
            Map.ChangeCellUnitType(0, i, UnitType.Wall);
            Map.ChangeCellUnitType(width + 1, i, UnitType.Wall);
        }
        // wall blocks
        for(int i = 1; i < width + 1; i++) {
            for(int j = 1; j < height + 1; j++) {
                if((i & 1) == 0 && (j & 1) == 0) {
                    Map.ChangeCellUnitType(i, j, UnitType.Wall);
                }
            }
        }
        GameController.Enemy = enemyCount;
        var random = new System.Random();
        while(enemyCount > 0) {
            var x = random.Next(1, width + 1);
            var z = random.Next(1, height + 1);

            if(Map.CellIsEmpty(x, z)) {
                Map.ChangeCellUnitType(x, z, UnitType.Enemy);
                enemyCount--;
            }
        }

        for(int i = 1; i < width + 1; i++) {
            for(int j = 1; j < height + 1; j++) {
                if((Random.Range(0f, 1f) <= 0.25f) && (Map.CellIsEmpty(i, j))) {
                    Map.ChangeCellUnitType(i, j, UnitType.WoodenWall);
                }
            }
        }
        GameController.WWall = Map[UnitType.WoodenWall].Length;

        foreach(var cellInfo in Map.GetCellsInsteadOf(UnitType.Empty, UnitType.Stub)) {
            var obj = GetObject(cellInfo.UnitType);
            obj.transform.position = new Vector3(-cellInfo.X, 0, cellInfo.Z) * BlockSize;
            Instantiate(obj);
        }
        Instantiate(player);
    }
    GameObject GetObject(UnitType unitType) {
        switch(unitType) {
            case UnitType.Wall:
                return wall;
            case UnitType.Enemy:
                return enemy1;
            case UnitType.WoodenWall:
                return wWall;
            default:
                throw new NotSupportedException();
        }
    }
}
