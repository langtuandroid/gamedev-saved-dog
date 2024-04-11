using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int levelNumberInGame;
    public int star;
    public bool Done = true;
    private Timer clockTimer; public Timer ClockTimer { get { return clockTimer; } }
    [SerializeField] private int LengthOfLevel;
    [SerializeField] private List<HealthDogeController> healthDogeList;
    [SerializeField] private List<Beehive> beehiveList;
    void Start()
    {
        star = 3;
        clockTimer = GameObject.FindObjectOfType<Timer>();
        if (clockTimer != null)
        {
            clockTimer.SetDuration(LengthOfLevel);
        }
    }
    public void OnInit()
    {
        clockTimer = GameObject.FindObjectOfType<Timer>();
        clockTimer.SetDuration(LengthOfLevel);
    }
    public void SetTime()
    {
        clockTimer = GameObject.FindObjectOfType<Timer>();
        if (clockTimer != null)
        {
            clockTimer.SetDuration(LengthOfLevel);
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
        return clockTimer.RemainingDuration;
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
