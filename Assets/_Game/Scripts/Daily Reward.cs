using System;
using System.Collections;
using UnityEngine;

public enum DayState
{
    Day1,
    Day2,
    Day3,
    Day4,
    Day5,
    Day6,
    Day7
};

public class DailyReward : MonoBehaviour
{
    public event Action onGainReward, onCanGainNextDay;
    public DayState currentDayState;
    public bool canClaim;

    private const string lastClaimTimeKey = "LastClaimTime";
    private const string isProgressKey = "isProgress";
    private const string dayStateKey = "DayStateKey";
    private const string lastWaitTimeKey = "lastWaitTime";
    private bool isProgress;
    private DateTime timerStart, timerEnd;
    private DateTime timerClaimStart, timerClaimEnd;
    private Coroutine waitCoroutine;

    [SerializeField] private int hours, minutes, seconds;

    #region Unity Methods
    private void Awake()
    {
        DateTime time = LoadLastClaimTime();
        DateTime timeWait = LoadLastWaitTime();
        isProgress = LoadState();
        currentDayState = (DayState)LoadDay();
        if (time != DateTime.MinValue)
        {
            timerEnd = time.Add(new TimeSpan(hours, minutes, seconds));
        }
        if (timeWait != DateTime.MinValue)
        {
            timerClaimEnd = timeWait.Add(new TimeSpan(hours, minutes, seconds));
        }
    }
    private void Start()
    {
        onGainReward += StartTimer;
        onCanGainNextDay += PassToNextDay;
        if (isProgress)
        {
            StartCoroutine(TimeCounter());
        }
        else
        {
            canClaim = true;
            waitCoroutine = StartCoroutine(WaitToClaim());
        }
    }
    private void OnDestroy()
    {
        onGainReward -= StartTimer;
        onCanGainNextDay -= PassToNextDay;
    }
    #endregion

    #region UI Methods
    #endregion

    #region Timed Event
    public void InvokeGainReward()
    {
        onGainReward?.Invoke();
    }
    private void StartTimer()
    {
        timerStart = DateTime.Now;
        SaveLastClaimTime();
        TimeSpan time = new TimeSpan(hours, minutes, seconds);
        timerEnd = timerStart.Add(time);
        isProgress = true;
        canClaim = false;
        SaveState();


        StartCoroutine(TimeCounter());
    }

    private IEnumerator TimeCounter()
    {
        DateTime start = DateTime.Now;
        double secondsToFinish = (timerEnd - start).TotalSeconds;
        canClaim = false;
        yield return new WaitForSeconds(Convert.ToSingle(secondsToFinish));

        if (waitCoroutine != null)
            StopCoroutine(waitCoroutine);
        isProgress = false;
        canClaim = true;
        SaveState();

        // wait player to claim
        timerClaimStart = DateTime.Now;
        SaveLastWaitTime();
        TimeSpan time = new TimeSpan(hours, minutes, seconds);
        timerClaimEnd = timerClaimStart.Add(time);
        waitCoroutine = StartCoroutine(WaitToClaim());
    }
    private IEnumerator WaitToClaim()
    {
        DateTime start = DateTime.Now;
        double secondsWait = (timerClaimEnd - start).TotalSeconds;

        yield return new WaitForSeconds(Convert.ToSingle(secondsWait));
        currentDayState = (DayState)0;
        SaveDay();
    }
    public void PassToNextDay()
    {
        int dayNum = (int)currentDayState;
        if (dayNum == Enum.GetValues(typeof(DayState)).Length - 1)
        {
            dayNum = 0;
        }
        else
        {
            dayNum++;
        }
        currentDayState = (DayState)dayNum;
        SaveDay();
    }
    #endregion

    #region Save/Load
    public void SaveLastClaimTime()
    {
        string lastClaimTimeString = timerStart.ToString("yyyy-MM-dd HH:mm:ss");
        PlayerPrefs.SetString(lastClaimTimeKey, lastClaimTimeString);
        PlayerPrefs.Save();
    }
    public DateTime LoadLastClaimTime()
    {
        string lastClaimTimeString = PlayerPrefs.GetString(lastClaimTimeKey, DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss"));
        return DateTime.ParseExact(lastClaimTimeString, "yyyy-MM-dd HH:mm:ss", null);
    }
    public void SaveState()
    {
        int intValue = isProgress ? 1 : 0;
        PlayerPrefs.SetInt(isProgressKey, intValue);
    }
    public bool LoadState()
    {
        if (!PlayerPrefs.HasKey(isProgressKey))
        {
            canClaim = true;
        }
        int intValue = PlayerPrefs.GetInt(isProgressKey);
        return intValue == 1;
    }
    public void SaveDay()
    {
        int dayNum = (int)currentDayState;
        PlayerPrefs.SetInt(dayStateKey, dayNum);
    }
    public int LoadDay() 
    {
        int dayNum = PlayerPrefs.GetInt(dayStateKey);
        return dayNum;
    }
    public void SaveLastWaitTime()
    {
        string lastWaitTimeString = timerClaimStart.ToString("yyyy-MM-dd HH:mm:ss");
        PlayerPrefs.SetString(lastWaitTimeKey, lastWaitTimeString);
        PlayerPrefs.Save();
    }
    public DateTime LoadLastWaitTime()
    {
        string lastWaitTimeString = PlayerPrefs.GetString(lastWaitTimeKey, DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss"));
        return DateTime.ParseExact(lastWaitTimeString, "yyyy-MM-dd HH:mm:ss", null);
    }
    #endregion 
}
