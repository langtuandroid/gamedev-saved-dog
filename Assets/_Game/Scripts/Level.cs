using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private int LengthOfLevel;
    [SerializeField] private List<HealthDogeController> healthDogeList;
    [SerializeField] private List<Beehive> beehiveList;

    private TimerUI clockTimerUI;
    private int _starsCount;
    private int _levelNumberInGame;
    
    public TimerUI ClockTimerUI => clockTimerUI;
    public int StarsCount => _starsCount;
    public int LevelNumberInGame => _levelNumberInGame;

    private void Start()
    {
        _starsCount = 3;
        clockTimerUI = FindObjectOfType<TimerUI>();
        if (clockTimerUI != null)
        {
            clockTimerUI.SetDurationOfLevel(LengthOfLevel);
        }
    }

    public void SetTime()
    {
        clockTimerUI = FindObjectOfType<TimerUI>();
        if (clockTimerUI != null)
        {
            clockTimerUI.SetDurationOfLevel(LengthOfLevel);
        }

    }

    public void SetStarsCount (int starsCount)
    {
        _starsCount = starsCount;
    }

    public void SetLevelNumberInGame (int levelNumber)
    {
        _levelNumberInGame = levelNumber;
    }
    
    public bool IsDogeAlive()
    {
        foreach (HealthDogeController doge in healthDogeList)
        {
            if (doge.CurrentHealth <= 0)
            {
                return false;
            }
        }

        return true;
    }
    public void SetWinAnimation()
    {
        foreach (HealthDogeController doge in healthDogeList)
        {
            doge.SetWinAnimation();
        }
    }
    public void DisableHealthBar()
    {
        foreach (HealthDogeController doge in healthDogeList)
        {
            doge.OffHealthBar();
        }
    }

    public void DestroyAllBees()
    {
        foreach (Beehive beehive in beehiveList)
        {
            beehive.DestroyAllBees();
        }
    }
    public void SetDogeSkin(int index, int hp)
    {
        foreach (HealthDogeController doge in healthDogeList)
        {
            doge.MaxHealth = hp;
            doge.gameObject.GetComponent<AnimationControllerDoge>().SetSkinForDoge(index);
        }
    }
}
