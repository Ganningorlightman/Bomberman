using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour {

    public GameObject gObject;
    public Vector3 position;
    public Vector3 localScale;
    public string model;
    public new string name;

    public void Create(Vector3 localScale, string model, string name)
    {
        gObject = (Resources.Load(model, typeof(GameObject))) as GameObject;
        gObject.transform.localScale = localScale;
        this.model = model;
        this.name = name;
    }
    public void InstantiateObject(Vector3 position)
    {
        this.position = position;
        gObject.transform.position = position;
        Instantiate(gObject);
    }
}
