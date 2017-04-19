using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenWall : MonoBehaviour {

    private int rand;
    private bool destroy = false;
    private GameObject UnderObject;

    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Explosion") && (!destroy))
        {
            destroy = true;
            Destroy(gameObject);
            GameController.WWall--;          
            if((GameController.WWall == 0) && (!GameController.ExitCreated))
            {
                UnderObject = ObjectLoader.getObject("Models/Exit");                
                UnderObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Instantiate(UnderObject);
                GameController.ExitCreated = true;
            }
            else
            {
                rand = Random.Range(0, 7);
                if((rand == 1) && (!GameController.ExitCreated))
                {
                    UnderObject = ObjectLoader.getObject("Models/Exit");
                    UnderObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    Instantiate(UnderObject);
                    GameController.ExitCreated = true;
                }
                else if ((rand == 3) && (GameController.BonusesOnLevel > 0))
                {

                    UnderObject = ObjectLoader.getObject("Models/" + ChooseBonus());
                    UnderObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    Instantiate(UnderObject);
                    GameController.BonusesOnLevel--;
                }
            }                  
        }
    }

    public string ChooseBonus()
    {
        rand = Random.Range(0, GameController.BonusPower);

        switch (rand)
        {
            case 0: return "Bombs";               
            case 1: return "Flames";
            case 2: return "Speed";
            case 3: return "WallPass";
            case 4: return "Detonator";
            case 5: return "BombPass";
            case 6: return "FlamePass";

            default: return "Flames";
        }
    }

}
