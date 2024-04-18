using System.Collections.Generic;

[System.Serializable]
public class GameData  
{
    public int coin;
    public int currentLevelInProgress;
    public int currentChar;

    public bool music, sound, vibrate;

    public List<int> currentAd;
    public List<int> maxAd;
    public List<int> charUnlock;
    public List<int> levelDoneInGame;
    public List<int> starDoneInLevels;
    
    public GameData()
    {
        music = false;
        sound = false;
        vibrate = false;
        
        coin = 0;
        currentChar = 0;
        currentLevelInProgress = 0;
        
        charUnlock = new List<int>();
        currentAd = new List<int>();
        maxAd = new List<int>();
        levelDoneInGame = new List<int>();
        starDoneInLevels = new List<int>();
    }
}
