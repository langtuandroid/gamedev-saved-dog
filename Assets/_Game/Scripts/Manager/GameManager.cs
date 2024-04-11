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

   [Inject]
   private void Construct (LevelManager levelManager, DataPersistence dataPersistence)
   {
       _levelManager = levelManager;
       _dataPersistence = dataPersistence;
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
            UIManager.Instance.OpenUI<UIGameplay>();
            _levelManager.OnLoadLevel(0);
        }
        else
        {
            ChangeState(GameState.MainMenu);
            UIManager.Instance.OpenUI<UIMainMenu>();
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
        UIManager.Instance.CloseUI<UIGameplay>();
        if (_levelManager.currentLevel.levelNumberInGame > 1)
        {
            StartCoroutine(iShowWin());
        }
        else
        {
            UIManager.Instance.OpenUI<UIWin>();
        }
        _dataPersistence.SaveGame();
    }

    IEnumerator iShowWin()
    {
        yield return new WaitForSeconds(1);
        UIManager.Instance.OpenUI<UIWin>();
    }

    public void WhenLose()
    {
        ChangeState(GameState.Lose);
        blade.gameObject.SetActive(false);

        _levelManager.currentLevel.ClockTimer.StopClock();

        AudioManager.instance.Play(Constant.AUDIO_SFX_LOSE);

        UIManager.Instance.GetUI<UIGameplay>().tickLose.gameObject.SetActive(true);

        StartCoroutine(StartScale());

        _dataPersistence.SaveGame();

    }

    private IEnumerator StartScale()
    {
        scaleTween = UIManager.Instance.GetUI<UIGameplay>().tickLose.rectTransform.DOScale(8f, 2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(2f);

        // new version lose state
        _levelManager.OnLose();
        UIManager.Instance.CloseUI<UIGameplay>();
        if (_levelManager.currentLevel.levelNumberInGame > 1)
        {
            StartCoroutine(iShowLose());
        }
        else
        {
            UIManager.Instance.OpenUI<UILose>();
        }
        // old version lose state
        //UIManager.Instance.GetUI<UIGameplay>().OnRetryButton();
    }

    IEnumerator iShowLose()
    {
        yield return new WaitForSeconds(1);
         UIManager.Instance.OpenUI<UILose>();
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
