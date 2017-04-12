using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoader : MonoBehaviour {

    public static GameObject getObject(string path)
    {
        return (Resources.Load("Models/" + path, typeof(GameObject))) as GameObject;
    }
}
