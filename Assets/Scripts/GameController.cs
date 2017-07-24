using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameController {

    public static bool ExitOpen { get { return Enemy == 0; } }
    public static int Enemy = 0;
    public static int Score = 0;
    public static int WWall = 0;
    public static int BonusesOnLevel = 2;
    public static int BonusPower = 2;
    public static int Level = 0;
    public static bool ExitCreated = false;
}
public static class PlayerCharacteristics {
    public static int MoveSpeed = 7;
    public static int Bombs = 1;
    public static int Flames = 1;
    public static bool WallPass = false;
    public static bool BombPass = false;
    public static bool FlamePass = false;
    public static bool Detonator = false;
    //public PlayerCharacteristics(int moveSpeed, int bombs, int flames, bool wallPass, bool bombPass, bool flamePass, bool detonator) {
    //    MoveSpeed = moveSpeed;
    //    Bombs = bombs;
    //    Flames = flames;
    //    WallPass = wallPass;
    //    BombPass = bombPass;
    //    FlamePass = flamePass;
    //    Detonator = detonator;
    //}
}
