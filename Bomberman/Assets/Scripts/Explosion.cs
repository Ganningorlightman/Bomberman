using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

  
	void Start () {
        
	}
	
	void Update () {
        Destroy(gameObject, 1f);
	}

}
