using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class Timer : MonoBehaviour
{
    [SerializeField] public Image clockFill;
    [SerializeField] public TextMeshProUGUI timeText;

    private int duration;

    Coroutine coroutineInstance;

    private float remainingDuration; public float RemainingDuration { get { return remainingDuration; } }
    private float fillAmount;
    void Start()
    {
        LinesDrawer.instance.OnEndDraw += CallClock;

        fillAmount = 1f;
    }
    private void OnDestroy()
    {
        LinesDrawer.instance.OnEndDraw -= CallClock;
    }
    private void CallClock()
    {
        SetupClock(duration);
    }

    private void SetupClock(int duration)
    {
        SetUIClockTrue();
        remainingDuration = duration;
        coroutineInstance = StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while(remainingDuration >= 0)
        {
            timeText.text = $"{remainingDuration % 60}";
            clockFill.fillAmount = Mathf.InverseLerp(0, duration, remainingDuration);

            remainingDuration--;
            yield return new WaitForSeconds(1f);
        }

        // Check dieu kien win
        Level currentLevel = GameObject.FindObjectOfType<Level>();
        if (currentLevel.DogeStillAlive())
        {
            GameManager.Instance.currentIndexState = LevelManager.Instance.stateIndex;
            currentLevel.SetAnimWin();
            currentLevel.TurnOffHealthBar();
            LevelManager.Instance.currentLevel.DestroyAllBees();

            AudioManager.instance.Play(Constant.AUDIO_SFX_WOOHOO);

            AnimTickWinThenWin();
        }
    }

    private void AnimTickWinThenWin()
    {
        UIManager.Instance.GetUI<UIGameplay>().tickWin.gameObject.SetActive(true);
        UIManager.Instance.GetUI<UIGameplay>().tickWin.rectTransform.DOScale(8f, 2f).SetEase(Ease.InOutSine).OnComplete(GameManager.Instance.WhenVictory);
    }

    public void SetDuration(int duration)
    {
        this.duration = duration;
        timeText.text = $"{duration % 60}";
        fillAmount = 1f;
        clockFill.fillAmount = fillAmount;
        if (coroutineInstance != null)
        {
            StopClock();
        }
    }
    public void SetUIClockFalse()
    {
        clockFill.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
    }
    public void SetUIClockTrue()
    {
        clockFill.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
    }
    public void StopClock()
    {
        StopCoroutine(coroutineInstance);
    }
}
