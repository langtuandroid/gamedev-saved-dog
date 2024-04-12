using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int levelNumberInGame;
    public int star;
    public bool Done = true;
    private TimerUI clockTimerUI; public TimerUI ClockTimerUI { get { return clockTimerUI; } }
    [SerializeField] private int LengthOfLevel;
    [SerializeField] private List<HealthDogeController> healthDogeList;
    [SerializeField] private List<Beehive> beehiveList;
    void Start()
    {
        star = 3;
        clockTimerUI = GameObject.FindObjectOfType<TimerUI>();
        if (clockTimerUI != null)
        {
            clockTimerUI.SetDurationOfLevel(LengthOfLevel);
        }
    }
    public void OnInit()
    {
        clockTimerUI = GameObject.FindObjectOfType<TimerUI>();
        clockTimerUI.SetDurationOfLevel(LengthOfLevel);
    }
    public void SetTime()
    {
        clockTimerUI = GameObject.FindObjectOfType<TimerUI>();
        if (clockTimerUI != null)
        {
            clockTimerUI.SetDurationOfLevel(LengthOfLevel);
        }
        //clockTimer.SetUIClockFalse();
    }
    public bool DogeStillAlive()
    {
        for (int i = 0; i < healthDogeList.Count; i++)
        {
            if (healthDogeList[i].CurrentHealth <= 0)
            {
                return false;
            }
        }
        return true;
    }
    public void SetAnimWin()
    {
        for (int i = 0; i < healthDogeList.Count; i++)
        {
            healthDogeList[i].SetAnimWin();
        }
    }
    public void TurnOffHealthBar()
    {
        for (int i = 0; i < healthDogeList.Count; i++)
        {
            healthDogeList[i].OffHealthBar();
        }
    }
    public float GetTimeRemain()
    {
        return clockTimerUI.RemainingDuration;
    }
    public void DestroyAllBees()
    {
        for (int i = 0; i < beehiveList.Count; i++)
        {
            beehiveList[i].DestroyAllBees();
        }
    }
    public void SetSkin(int index, int hp)
    {
        for (int i = 0; i < healthDogeList.Count; i++)
        {
            healthDogeList[i].MaxHealth = hp;
            healthDogeList[i].gameObject.GetComponent<AnimationControllerDoge>().SetSkinForDoge(index);     
        }
    }
}
