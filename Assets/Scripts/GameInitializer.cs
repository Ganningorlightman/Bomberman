using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
public class Map {

    public readonly int Width;
    public readonly int Height;
    readonly List<CellInfo> cells;

    public Map(int width, int height) {
        Width = width;
        Height = height;
        cells = new List<CellInfo>();
        Initialize();
    }
    public CellInfo[] this[UnitType unitType] {
        get {
            return cells.Where(x => x.UnitType == unitType).ToArray();
        }
    }
    private void Initialize() {
        for(int i = 0; i < Width + 2; i++) {
            for(int j = 0; j < Height + 2; j++) {
                var cellInfo = new CellInfo(i, j, UnitType.Empty);
                cells.Add(cellInfo);
            }
        }
    }
    public void ChangeCellUnitType(float x, float z, UnitType unitType) {
        cells.Single(cellInfo => cellInfo.X == x && cellInfo.Z == z).UnitType = unitType;
    }
    public bool CellIsEmpty(float x, float z) {
        return cells.Single(cellInfo => cellInfo.X == x && cellInfo.Z == z).UnitType == UnitType.Empty;
    }
    public CellInfo[] GetCellsInsteadOf(params UnitType[] unitTypes) {
        return cells.Where(x => unitTypes.All(unitType => x.UnitType != unitType)).ToArray();
    }
}
public class CellInfo {
    public CellInfo(float x, float z, UnitType unitType) {
        X = x;
        Z = z;
        UnitType = unitType;
    }

    public float X;
    public float Z;
    public UnitType UnitType;
}
public enum UnitType {
    Floor,
    Wall,
    WoodenWall,
    Enemy,
    Stub,
    Empty
}
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

        var map = new Map(width, height);
        map.ChangeCellUnitType(1, 1, UnitType.Stub);
        map.ChangeCellUnitType(1, 2, UnitType.Stub);
        map.ChangeCellUnitType(2, 1, UnitType.Stub);
        // map boder
        for(int i = 1; i < width + 1; i++) {
            map.ChangeCellUnitType(i, 0, UnitType.Wall);
            map.ChangeCellUnitType(i, height + 1, UnitType.Wall);
        }
        for(int i = 0; i < height + 2; i++) {
            map.ChangeCellUnitType(0, i, UnitType.Wall);
            map.ChangeCellUnitType(width + 1, i, UnitType.Wall);
        }
        // wall blocks
        for(int i = 1; i < width + 1; i++) {
            for(int j = 1; j < height + 1; j++) {
                if((i & 1) == 0 && (j & 1) == 0) {
                    map.ChangeCellUnitType(i, j, UnitType.Wall);
                }
            }
        }
        GameController.Enemy = enemyCount;
        var random = new System.Random();
        while(enemyCount > 0) {
            var x = random.Next(1, width + 1);
            var z = random.Next(1, height + 1);

            if(map.CellIsEmpty(x, z)) {
                map.ChangeCellUnitType(x, z, UnitType.Enemy);
                enemyCount--;
            }
        }

        for(int i = 1; i < width + 1; i++) {
            for(int j = 1; j < height + 1; j++) {
                if((Random.Range(0f, 1f) <= 0.25f) && (map.CellIsEmpty(i, j))){
                    map.ChangeCellUnitType(i, j, UnitType.WoodenWall);                 
                }
            }
        }
        GameController.WWall = map[UnitType.WoodenWall].Length;

        foreach(var cellInfo in map.GetCellsInsteadOf(UnitType.Empty, UnitType.Stub)) {          
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
