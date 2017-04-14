using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	void Update () {
        Destroy(gameObject, 0.7f);
	}
}
