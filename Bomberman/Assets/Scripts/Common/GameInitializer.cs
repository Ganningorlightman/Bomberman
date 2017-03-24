using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour {

   public GameObject walls;
   public GameObject floor;

    void Start ()
    {

        walls = (Resources.Load("Walls/Wall", typeof(GameObject))) as GameObject;
        walls.transform.localScale = new Vector3(5f, 5f, 5f);

        for (int i = 0; i < 11 * 5; i += 5)
            for(int j = 0; j < 13 * 5; j += 5)
        {
                if ((j % 2 == 0)&&(i % 2 == 0))
                {
                    walls.transform.position = new Vector3(-j, 0, i);
                    Instantiate(walls);
                }
        }


        floor = (Resources.Load("Walls/Floor", typeof(GameObject))) as GameObject;
        floor.transform.position = new Vector3(-30f, -2.5f, 25f);
        floor.transform.localScale = new Vector3(6.5f, 1f, 5.5f);

        Instantiate(floor);

    }
    void Update () {
		
	}
}
