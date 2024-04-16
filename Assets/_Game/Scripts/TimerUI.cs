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
    private LinesDrawer _linesDrawer;
    
    public float RemainingDuration => _remainingDuration;

    [Inject]
    private void Construct(GameManager gameManager, LevelManager levelManager, AudioManager audioManager, UIManager uiManager, LinesDrawer linesDrawer)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;
        _audioManager = audioManager;
        _uiManager = uiManager;
        _linesDrawer = linesDrawer;
    }

    private void Start()
    {
        _linesDrawer.OnEndDraw += CallTimer;

        _fillAmount = 1f;
    }
    
    private void OnDestroy()
    {
        _linesDrawer.OnEndDraw -= CallTimer;
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

        if (!currentLevel.IsDogeAlive())
        {
            yield break;
        }

        _gameManager.SetCurrentIndex(_levelManager.StateIndex);
        currentLevel.SetWinAnimation();
        currentLevel.DisableHealthBar();
        _levelManager.CurrentLevel.DestroyAllBees();

        _audioManager.Play(Constant.AUDIO_SFX_WOOHOO);

        AnimTickWinThenWin();
    }

    private void AnimTickWinThenWin()
    {
        _uiManager.GetUI<UIGameplay>().TickWin.gameObject.SetActive(true);
        _uiManager.GetUI<UIGameplay>().TickWin.rectTransform.DOScale(8f, 2f).SetEase(Ease.InOutSine).OnComplete(_gameManager.Victory);
    }

    private void SetUITimerTrue()
    {
        _timeFill.gameObject.SetActive(true);
        _timeText.gameObject.SetActive(true);
    }
}
