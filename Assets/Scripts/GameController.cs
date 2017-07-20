using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameController {

    public static bool ExitOpen { get { return Enemy == 0; } }
    public static int Enemy = 0;
    public static int Score = 0;
    public static int WWall = 14;
    public static int BonusesOnLevel = 2;
    public static int BonusPower = 2;
    public static bool ExitCreated = false;   
}
