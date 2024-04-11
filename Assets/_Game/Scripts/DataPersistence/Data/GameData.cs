using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData  
{
    public int coin;
    public int currentLevelInProgress;
    public int currentChar;
    public int totalStarDoneInGame;

    public bool music, sound, vibrate;

    public List<int> currentAd;
    public List<int> maxAd;
    public List<int> charUnlock;
    public List<int> totalStarDoneInActs;
    public List<int> levelDoneInGame;
    public List<int> starDoneInLevels;
    public GameData()
    {
        this.music = false;

        this.sound = false;

        this.vibrate = false;

        this.coin = 0;                                     // v

        this.currentChar = 0;                               // v

        this.totalStarDoneInGame = 0;

        this.currentLevelInProgress = 0;                    // v

        this.charUnlock = new List<int>();                  // v

        this.currentAd = new List<int>();                   // v

        this.maxAd = new List<int>();                       // v

        this.totalStarDoneInActs = new List<int>();

        this.levelDoneInGame = new List<int>();

        this.starDoneInLevels = new List<int>();
    }
}
