using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public void SlowTime()
    {
        Time.timeScale = 0.25f;
    }
    
    public void NormalTime()
    {
        Time.timeScale = 1f;
    }
}
