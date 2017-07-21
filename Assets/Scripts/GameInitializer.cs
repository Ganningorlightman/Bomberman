using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
public class Map {

    readonly int width;
    public int Width { get { return width + 2; } }
    readonly int height;
    public int Height { get { return height + 2; } }
    readonly List<CellInfo> cells;

    public Map(int width, int height) {
        this.width = width;
        this.height = height;
        cells = new List<CellInfo>();
        Initialize();
    }
    public CellInfo[] this[UnitType unitType] {
        get {
            return cells.Where(x => x.UnitType == unitType).ToArray();
        }
    }
    private void Initialize() {
        for(int i = 0; i < Width; i++) {
            for(int j = 0; j < Height; j++) {
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

    public CellInfo GetStaticCell(float x, float z) {
        var foundCell = cells.Single(cell => cell.X == x && cell.Z == z);
        return new CellInfo(foundCell.X, foundCell.Z, CoerceUnitType(foundCell.UnitType));
    }

    UnitType CoerceUnitType(UnitType unitType) {
        switch(unitType) {
            case UnitType.Enemy:
            case UnitType.Stub:
            case UnitType.WoodenWall:
            case UnitType.Empty:
                return UnitType.Empty;
            case UnitType.Wall:
                return UnitType.Wall;
            default:
                throw new NotSupportedException();
        }
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

    public static Map Map;

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
        ////// TODO: REMOVE THIS SHIT
        int x1 = 0;
        int y1 = 0;
        ////// TODO: REMOVE THIS SHIT
        while(enemyCount > 0) {
            var x = random.Next(1, width + 1);
            var z = random.Next(1, height + 1);

            if(Map.CellIsEmpty(x, z)) {
                Map.ChangeCellUnitType(x, z, UnitType.Enemy);
                x1 = x;
                y1 = z;
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

        PathFinder.InitializeNodeArray();
        var s = PathFinder.FindPath(new Vector3(x1, 0, y1), new Vector3(1, 0, 1));

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
