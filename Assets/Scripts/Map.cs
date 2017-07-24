using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    public CellInfo GetStaticCell(float x, float z, bool wallPass) {
        var foundCell = cells.Single(cell => cell.X == x && cell.Z == z);
        return new CellInfo(foundCell.X, foundCell.Z, CoerceUnitType(foundCell.UnitType, wallPass));
    }

    UnitType CoerceUnitType(UnitType unitType, bool wallPass) {
        if(!wallPass && UnitType.WoodenWall == unitType) return UnitType.Wall;
        switch(unitType) {
            case UnitType.Enemy:
            case UnitType.Stub:
            case UnitType.WoodenWall:
            case UnitType.Empty:
                return UnitType.Empty;
            case UnitType.Wall:
            case UnitType.Bomb:
                return UnitType.Wall;
            default:
                throw new NotSupportedException();
        }
    }
}
