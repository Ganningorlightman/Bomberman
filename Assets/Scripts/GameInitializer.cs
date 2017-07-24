using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
public class GameInitializer : MonoBehaviour {

    public Map Map;

    public GameObject floor;
    public GameObject wall;
    public GameObject wWall;
    public GameObject player;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    public static int BlockSize = 5;
    public int MapWidth = 17;
    public int MapHeight = 9;
    public Vector3 cameraPosition;
    public LevelGenerator levelGenerator;

    void Start() {
        Instantiate(ObjectLoader.GetObject("Models/Directional Light"));
        levelGenerator = new LevelGenerator(GameController.Level);
        GenerateMap(MapWidth, MapHeight);
        cameraPosition = transform.position;
    }
    void OnGUI() {
        GUI.Label(new Rect(10, 10, 100, 100), "Score: " + GameController.Score + Environment.NewLine + "Level: " + (GameController.Level + 1));
    }
    void GenerateMap(int width, int height) {
        if((width & 1) == 0 || (height & 1) == 0) {
            throw new InvalidOperationException("Width or Height must be odd");
        }
        floor = ObjectLoader.GetObject("Models/Floor");
        wall = ObjectLoader.GetObject("Models/Wall");
        wWall = ObjectLoader.GetObject("Models/WWall");
        player = ObjectLoader.GetObject("Models/Player1");
        enemy1 = ObjectLoader.GetObject("Models/Enemy1");
        enemy2 = ObjectLoader.GetObject("Models/Enemy2");
        enemy3 = ObjectLoader.GetObject("Models/Enemy3");

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
        int enemyCount = 0;
        foreach(int i in levelGenerator.enemy) {
            enemyCount += i;
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
    void LateUpdate() {
        transform.position = cameraPosition;
    }
    GameObject GetObject(UnitType unitType) {
        switch(unitType) {
            case UnitType.Wall:
                return wall;
            case UnitType.Enemy:
                if(levelGenerator.enemy[0] > 0) {
                    levelGenerator.enemy[0]--;
                    return enemy1;
                } else if(levelGenerator.enemy[1] > 0) {
                    levelGenerator.enemy[1]--;
                    return enemy2;
                } else {
                    levelGenerator.enemy[2]--;
                    return enemy3;
                }         
            case UnitType.WoodenWall:
                return wWall;
            default:
                throw new NotSupportedException();
        }
    }
}
