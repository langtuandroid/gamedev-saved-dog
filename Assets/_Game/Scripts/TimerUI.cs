using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Zenject;

public class TimerUI : MonoBehaviour
{
    [SerializeField]
    public Image _timeFill;
    [SerializeField]
    public TextMeshProUGUI _timeText;

    private float _remainingDuration;
    private float _fillAmount;
    private int _duration;
    
    private Coroutine _coroutineInstance;
    private GameManager _gameManager;
    private LevelManager _levelManager;
    private AudioManager _audioManager;
    private UIManager _uiManager;
    
    public float RemainingDuration => _remainingDuration;

    [Inject]
    private void Construct(GameManager gameManager, LevelManager levelManager, AudioManager audioManager, UIManager uiManager)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;
        _audioManager = audioManager;
        _uiManager = uiManager;
    }

    private void Start()
    {
        LinesDrawer.instance.OnEndDraw += CallTimer;

        _fillAmount = 1f;
    }
    
    private void OnDestroy()
    {
        LinesDrawer.instance.OnEndDraw -= CallTimer;
    }
    
    public void SetDurationOfLevel(int duration)
    {
        _duration = duration;
        _timeText.text = $"{duration % 60}";
        _fillAmount = 1f;
        _timeFill.fillAmount = _fillAmount;
        if (_coroutineInstance != null)
        {
            StopTimer();
        }
    }

    public void StopTimer()
    {
        StopCoroutine(_coroutineInstance);
    }
    
    private void CallTimer()
    {
        SetupTimer(_duration);
    }

    private void SetupTimer(int duration)
    {
        SetUITimerTrue();
        _remainingDuration = duration;
        _coroutineInstance = StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while(_remainingDuration >= 0)
        {
            _timeText.text = $"{_remainingDuration % 60}";
            _timeFill.fillAmount = Mathf.InverseLerp(0, _duration, _remainingDuration);

            _remainingDuration--;
            yield return new WaitForSeconds(1f);
        }

        Level currentLevel = FindObjectOfType<Level>();

        if (!currentLevel.DogeStillAlive())
        {
            yield break;
        }

        _gameManager.currentIndexState = _levelManager.stateIndex;
        currentLevel.SetAnimWin();
        currentLevel.TurnOffHealthBar();
        _levelManager.currentLevel.DestroyAllBees();

        _audioManager.Play(Constant.AUDIO_SFX_WOOHOO);

        AnimTickWinThenWin();
    }

    private void AnimTickWinThenWin()
    {
        _uiManager.GetUI<UIGameplay>().tickWin.gameObject.SetActive(true);
        _uiManager.GetUI<UIGameplay>().tickWin.rectTransform.DOScale(8f, 2f).SetEase(Ease.InOutSine).OnComplete(_gameManager.WhenVictory);
    }

    private void SetUITimerTrue()
    {
        _timeFill.gameObject.SetActive(true);
        _timeText.gameObject.SetActive(true);
    }
}