using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar {

    public List<Step> OpenList;
    public List<Step> CloseList;

    public void FindPath(Vector3 start, Vector3 end, Map map) {
        OpenList = new List<Step>();
        CloseList = new List<Step>();
        OpenList.Add(new Step(start));

        while(Check(new Step(end), OpenList)) {
            int i = MinInOpenList(OpenList);
            CloseList.Add(OpenList[i]);
            int j = CloseList.IndexOf(OpenList[i]);
            OpenList.RemoveAt(i);

        }

    }
    public int MinInOpenList(List<Step> openList) {
        int ind = 0;
        int min = openList[0].G + openList[0].H;
        foreach(var step in openList) {
            if(min > (step.G + step.H)) {
                ind = openList.IndexOf(step);
                min = step.G + step.H;
            }
        }
        return ind;
    }
    public bool Check(Step end, List<Step> openList) {
        return !((openList.Count == 0) || (openList.IndexOf(end) != -1));
    }
}
public class Step {
    public Vector3 Coordinate;
    public int G;
    public int H;
    public int Parent;

    public Step(Vector3 coordinate) {
        Coordinate = coordinate;
        G = 0;
        H = 0;
    }
    public int GetDistance(Step start, Step end, Map map) {
        return 0;
    }
}
