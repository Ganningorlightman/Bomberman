using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

