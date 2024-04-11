using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    
    public void SlowTime()
    {
        Time.timeScale = 0.25f;
    }
    public void BackNormalTime()
    {
        Time.timeScale = 1f;
    }
}
