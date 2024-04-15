using System.Collections;
using UnityEngine;
using DG.Tweening;
using Zenject;

public enum GameState
{
    MainMenu,
    GamePlay,
    Win, 
    Setting,
    Skin,
    Lose
}

public class GameManager : MonoBehaviour
{
    private const string FIRST_LOAD = "firstload";

    public int currentIndexState;

    [SerializeField]
    private Blade blade;
    
    private GameState gameState;
    private Tweener scaleTween;
    private LevelManager _levelManager;
    private DataPersistence _dataPersistence;
    private AudioManager _audioManager;
    private UIManager _uiManager;

   [Inject]
   private void Construct (LevelManager levelManager, DataPersistence dataPersistence, AudioManager audioManager, UIManager uiManager)
   {
       _levelManager = levelManager;
       _dataPersistence = dataPersistence;
       _audioManager = audioManager;
       _uiManager = uiManager;
   }

    protected void Awake()
    {
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        int maxScreenHeight = 1280;
        float ratio = Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }

        if (!PlayerPrefs.HasKey(FIRST_LOAD))
        {
            PlayerPrefs.SetInt(FIRST_LOAD, 1);
            ChangeState(GameState.GamePlay);
            _uiManager.OpenUI<UIGameplay>();
            _levelManager.OnLoadLevel(0);
        } else
        {
            ChangeState(GameState.MainMenu);
            _uiManager.OpenUI<UIMainMenu>();
        }

    }

    public void WhenVictory()
    {

        blade.gameObject.SetActive(false);

        _levelManager.OnWin();

        if (currentIndexState != _levelManager.stateIndex)
        {
            currentIndexState = _levelManager.stateIndex;
            return;
        }

        ChangeState(GameState.Win);
        _uiManager.CloseUI<UIGameplay>();
        if (_levelManager.currentLevel.LevelNumberInGame > 1)
        {
            StartCoroutine(ShowWin());
        } else
        {
            _uiManager.OpenUI<UIWin>();
        }
        _dataPersistence.SaveGame();
    }
    
    public void WhenLose()
    {
        ChangeState(GameState.Lose);
        blade.gameObject.SetActive(false);

        _levelManager.currentLevel.ClockTimerUI.StopTimer();

        _audioManager.Play(Constant.AUDIO_SFX_LOSE);

        _uiManager.GetUI<UIGameplay>().tickLose.gameObject.SetActive(true);

        StartCoroutine(StartScale());

        _dataPersistence.SaveGame();

    }

    private IEnumerator StartScale()
    {
        scaleTween = _uiManager.GetUI<UIGameplay>().tickLose.rectTransform.DOScale(8f, 2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(2f);

        _levelManager.OnLose();
        _uiManager.CloseUI<UIGameplay>();
        if (_levelManager.currentLevel.LevelNumberInGame > 1)
        {
            StartCoroutine(ShowLose());
        } else
        {
            _uiManager.OpenUI<UILose>();
        }
    }
    
    private IEnumerator ShowWin()
    {
        yield return new WaitForSeconds(1);
        _uiManager.OpenUI<UIWin>();
    }

    private IEnumerator ShowLose()
    {
        yield return new WaitForSeconds(1);
        _uiManager.OpenUI<UILose>();
    }

    public void ChangeState(GameState state)
    {
        gameState = state;
    }

    public bool IsState(GameState state)
    {
        return gameState == state;
    }
    public void StopTween()
    {
        scaleTween?.Kill();
    }
}
