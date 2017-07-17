using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoader : MonoBehaviour {

    public static GameObject GetObject(string path) {
        return (Resources.Load(path, typeof(GameObject))) as GameObject;
    }
}
