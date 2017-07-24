using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator{
    public int[] enemy;
    public LevelGenerator(int level) {
        enemy = new int[3];
        if(level < 5) {
            enemy[0] = 3 + level * 2;
            enemy[1] = 1 + level;
            enemy[2] = level;
            GameController.BonusesOnLevel = 2;
            GameController.BonusPower = 3;
        }
        else if ((level > 5) && (level < 10)){
            enemy[0] = 3 + level;
            enemy[1] = 5 + level;
            enemy[2] = 1 + level;
            GameController.BonusesOnLevel = 3;
            GameController.BonusPower = 5;
        }
        else {
            enemy[0] = 0;
            enemy[1] = level * 2;
            enemy[2] = 3 + level;
            GameController.BonusesOnLevel = 2;
            GameController.BonusPower = 7;
        }
    }
}
