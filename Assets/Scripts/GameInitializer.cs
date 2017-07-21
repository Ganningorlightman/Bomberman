using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

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
    public int MapWidth = 7;
    public int MapHeight = 7;

    void Start() {
        Instantiate(ObjectLoader.GetObject("Models/Directional Light")); 
        GenerateMap(MapWidth, MapHeight, 3);
    }

    void OnGUI() {
        GUI.Label(new Rect(10, 10, 100, 100), "Score: " + GameController.Score);
    }

    enum UnitType {
        Floor,
        Wall,
        WoodenWall,
        Enemy,
        Stub,
        Empty
    }
    struct CellInfo {
        public CellInfo(float x, float z, UnitType unitType) {
            X = x;
            Z = z;
            UnitType = unitType;
        }

        public float X;
        public float Z;
        public UnitType UnitType;
    }
    class Map : List<CellInfo> {
        public CellInfo[] this[UnitType unitType] {
            get {
                return this.Where(x => x.UnitType == unitType).ToArray();
            }
        }

        public bool CellIsEmpty(float x, float z) {
            return !this.Any(cellInfo => cellInfo.X == x && cellInfo.Z == z);
        }
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

        var map = new Map();

        map.Add(new CellInfo(-1, 1, UnitType.Stub));
        map.Add(new CellInfo(-1, 2, UnitType.Stub));
        map.Add(new CellInfo(-2, 1, UnitType.Stub));
        // map boder
        for(int i = 0; i < height + 2; i++) {
            var cellInfo = new CellInfo(0, i, UnitType.Wall);
            map.Add(cellInfo);
            cellInfo = new CellInfo(-(width + 1), i, UnitType.Wall);
            map.Add(cellInfo);
        }
        for(int i = 1; i < width + 1; i++) {
            var cellInfo = new CellInfo(-i, 0, UnitType.Wall);
            map.Add(cellInfo);
            cellInfo = new CellInfo(-i, height + 1, UnitType.Wall);
            map.Add(cellInfo);
        }
        // wall blocks
        for(int i = 1; i < height + 1; i++) {
            for(int j = 1; j < width + 1; j++) {
                if((i & 1) == 0 && (j & 1) == 0) {
                    var cellInfo = new CellInfo(-j, i, UnitType.Wall);
                    map.Add(cellInfo);
                }
            }
        }
        GameController.Enemy = enemyCount;
        var random = new System.Random();
        while(enemyCount > 0) {
            var x = random.Next(1, width + 1);
            var z = random.Next(1, height + 1);

            if(map.CellIsEmpty(-x, z)) {
                var cellInfo = new CellInfo(-x, z, UnitType.Enemy);
                map.Add(cellInfo);
                enemyCount--;
            }
        }

        for(int i = 1; i < height + 1; i++) {
            for(int j = 1; j < width + 1; j++) {
                if((Random.Range(0f, 1f) <= 0.25f) && (map.CellIsEmpty(-j, i))){
                    var cellInfo = new CellInfo(-j, i, UnitType.WoodenWall);
                    map.Add(cellInfo);
                    GameController.WWall++;
                }
            }
        }
        
        foreach(var cellInfo in map.Where(x => x.UnitType != UnitType.Stub)) {          
                var obj = GetObject(cellInfo.UnitType);
                obj.transform.position = new Vector3(cellInfo.X, 0, cellInfo.Z) * BlockSize;
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
