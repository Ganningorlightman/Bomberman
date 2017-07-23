using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Heap {
    public Heap(int maxHeapSize) {
        items = new IHeapItem[maxHeapSize];
    }

    readonly IHeapItem[] items;
    int currentItemCount;

    public void Add(IHeapItem item) {
        item.SetHeapIndex(currentItemCount);
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public IHeapItem RemoveFirst() {
        IHeapItem firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].SetHeapIndex(0);
        SortDown(items[0]);
        return firstItem;
    }

    public bool Contains(IHeapItem item) {
        if(items[item.GetHeapIndex()] == null)
            return false;
        return items[item.GetHeapIndex()].Equals(item);
    }

    public void UpdateItem(IHeapItem item) { SortUp(item); }

    public int GetCount() { return currentItemCount; }

    void SortDown(IHeapItem item) {
        while(true) {
            int childIndexLeft = item.GetHeapIndex() * 2 + 1;
            int childIndexRight = item.GetHeapIndex() * 2 + 2;
            if(childIndexLeft < currentItemCount) {
                var swapIndex = childIndexLeft;
                if(childIndexRight < currentItemCount)
                    if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        swapIndex = childIndexRight;
                if(item.CompareTo(items[swapIndex]) < 0)
                    Swap(item, items[swapIndex]);
                else
                    return;
            } else {
                return;
            }
        }
    }

    void SortUp(IHeapItem item) {
        int parentIndex = (item.GetHeapIndex() - 1) / 2;

        while(true) {
            IHeapItem parentItem = items[parentIndex];
            if(item.CompareTo(parentItem) > 0)
                Swap(item, parentItem);
            else
                break;
            parentIndex = (item.GetHeapIndex() - 1) / 2;
        }
    }

    void Swap(IHeapItem itemA, IHeapItem itemB) {
        items[itemA.GetHeapIndex()] = itemB;
        items[itemB.GetHeapIndex()] = itemA;
        int itemAIndex = itemA.GetHeapIndex();
        itemA.SetHeapIndex(itemB.GetHeapIndex());
        itemB.SetHeapIndex(itemAIndex);
    }
}

public interface IHeapItem : IComparable {
    int GetHeapIndex();
    void SetHeapIndex(int index);
}

public class Node : CellInfo, IHeapItem {
    public Node(int x, int y, UnitType unitType) : base(x, y, unitType) {
        GCost = 0;
        HCost = 0;
    }

    int heapIndex;

    public int GCost;
    public int HCost;
    public Node Parent;

    public int GetFCost() {
        return GCost + HCost;
    }

    int CompareInt(int a, int b) {
        if(a < b)
            return -1;
        if(a > b)
            return 1;
        return 0;
    }

    #region IComparable

    int IComparable.CompareTo(object o) {
        Node node = (Node)o;
        int compare = CompareInt(GetFCost(), node.GetFCost());
        if(compare == 0)
            compare = CompareInt(HCost, node.HCost);
        return -compare;
    }

    #endregion

    #region IHeapItem

    int IHeapItem.GetHeapIndex() { return heapIndex; }
    void IHeapItem.SetHeapIndex(int index) { heapIndex = index; }

    #endregion
}

public class PathFinder {
    public static int XPixels = 1;
    public static int YPixels = 1;

    static Node[][] NodeArray;
    public static void InitializeNodeArray() {
        NodeArray = new Node[GameInitializer.Map.Width / XPixels][];
        for(int i = 0; i < GameInitializer.Map.Width / XPixels; i++) {
            NodeArray[i] = new Node[GameInitializer.Map.Height / YPixels];
            for(int j = 0; j < GameInitializer.Map.Height / YPixels; j++)
                NodeArray[i][j] = new Node(i, j, GameInitializer.Map.GetStaticCell(i, j).UnitType);
        }
    }

    public static LinkedList<Node> GetNeighbours(Node node) {
        LinkedList<Node> neighbours = new LinkedList<Node>();
        for(int i = -1; i < 2; i++) {
            for(int j = -1; j < 2; j++) {
                if(Mathf.Abs(i) == Mathf.Abs(j))
                    continue;

                int checkX = (int)node.X + i;
                int checkY = (int)node.Z + j;

                if(checkX >= 0 && checkX < GameInitializer.Map.Width / XPixels && checkY >= 0 && checkY < GameInitializer.Map.Height / YPixels) {
                    var foundNode = NodeArray[checkX][checkY];
                    if(foundNode.UnitType != UnitType.Wall) {
                        neighbours.AddLast(foundNode);
                    }
                }
            }
        }

        return neighbours;
    }

    public static Stack<Node> FindPath(Vector3 start, Vector3 targer) {
        return new Stack<Node>(FindPathCore(start, targer));
    }

    private static Node[] FindPathCore(Vector3 start, Vector3 targer) {
        Node startNode = NodeArray[(int)(start.x / XPixels)][(int)(start.z / YPixels)];
        Node targetNode = NodeArray[(int)(targer.x / XPixels)][(int)(targer.z / YPixels)];

        Heap openSet = new Heap(GameInitializer.Map.Width * GameInitializer.Map.Height);
        LinkedList<Node> closedSet = new LinkedList<Node>();
        openSet.Add(startNode);

        while(openSet.GetCount() > 0) {
            Node currentNode = (Node)openSet.RemoveFirst();
            closedSet.AddLast(currentNode);

            if(currentNode == targetNode) {
                var path = new LinkedList<Node>();
                while(targetNode != startNode) {
                    path.AddLast(targetNode);
                    targetNode = targetNode.Parent;
                }
                return path.ToArray();
            }

            foreach(var neighbour in GetNeighbours(currentNode)) {
                if(closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour)) {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, targetNode);
                    neighbour.Parent = currentNode;

                    if(!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        return new Node[0];
    }

    public static int GetDistance(Node node1, Node node2) {
        int dstX = (int)Math.Abs(node1.X - node2.X);
        int dstY = (int)Math.Abs(node1.Z - node2.Z);
        if(dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
