using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;
using Zenject;

public class UIGameplay : UICanvas
{
    [SerializeField] private Image levelCompleteBar;
    [SerializeField] private RectTransform coinImageTransform;
    [SerializeField] private Transform timerBG;
    [SerializeField] private Sprite loseStarSprite;
    [SerializeField] private TextMeshProUGUI levelText, coinText;
    [SerializeField] private float maxInk;
    [SerializeField] private float inkLoseRate;
    [SerializeField] private Image[] starsImages;
    [SerializeField] private Image tickWin, tickLose;
    
    private float currentInk; 
    private float inkRatio;
    private Tween tween1, tween2, tween3;
    private Vector3 initPosClock;
    private int endValue;
    
    public float InkLoseRate => inkLoseRate;
    public float CurrentInk { get { return currentInk; } set { currentInk = value; } }
    public Image TickWin => tickWin;
    public Image TickLose => tickLose;

    private GameManager _gameManager;
    private LevelManager _levelManager;
    private DataController _dataController;
    private AudioManager _audioManager;
    private LinesDrawer _linesDrawer;

    [Inject]
    private void Construct (GameManager gameManager, LevelManager levelManager, DataController dataController, AudioManager audioManager, LinesDrawer linesDrawer)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;
        _dataController = dataController;
        _audioManager = audioManager;
        _linesDrawer = linesDrawer;
    }

    private void Awake()
    {
        initPosClock = timerBG.position;
    }

    private void OnEnable()
    {
        Init();

        _linesDrawer.OnEndDraw += PlayClockAnimation;
    }
    
    private void OnDisable()
    {
        _linesDrawer.OnEndDraw -= PlayClockAnimation;

        ResetClockAnimation();
    }

    public void Init()
    {
        levelCompleteBar.fillAmount = 1f;
        currentInk = maxInk;
        ColorStarReturnDefault();
        ResetTickWin();
        ResetTickLose();
        UpdateCoinText();
        ResetClockAnimation();
        ResetCoinImage();
    }

    public void HandleCoinGainInGameplay(int loop)
    {
        coinImageTransform.DOScale(1.2f, 0.1f).SetEase(Ease.Linear).SetLoops(loop, LoopType.Yoyo);

        endValue = _dataController.currentGameData.coin + loop/2;
        _dataController.currentGameData.coin += loop / 2;
        coinText.DOFade(0, 0.5f).From().SetEase(Ease.OutQuad).OnComplete(() =>
        {
            coinText.text = endValue.ToString();
            coinText.DOFade(1, 0.5f).SetEase(Ease.OutQuad);
        });
    }

    private void ResetCoinImage()
    {
        coinImageTransform.DOScale(1f, 0f);
    }
    
    private void PlayClockAnimation()
    {
        tween1 = timerBG.DOShakePosition(1f, 3f, 26, 90f, false).SetEase(Ease.InQuart).SetLoops(-1, LoopType.Yoyo);
        tween2 = timerBG.DOShakeRotation(1f, 10f, 10, 90f).SetEase(Ease.InQuart).SetLoops(-1, LoopType.Yoyo);
        tween3 = timerBG.DOShakeScale(1, 0.2f, 10, 90f).SetEase(Ease.InQuart).SetLoops(-1, LoopType.Yoyo);
    }

    private void ResetClockAnimation()
    {
        tween1.Kill();
        tween2.Kill();
        tween3.Kill();

        timerBG.transform.position = initPosClock;
        timerBG.transform.localRotation = Quaternion.identity;
        timerBG.transform.localScale = Vector3.one;
    }

    private void UpdateCoinText()
    {
        coinText.text = _dataController.currentGameData.coin.ToString();
    }
    private void ColorStarReturnDefault()
    {
        starsImages[1].color = starsImages[0].color;
        starsImages[2].color = starsImages[0].color;
    }

    public void UpdateInkBar()
    {
        levelCompleteBar.fillAmount = currentInk / maxInk;
        UpdateStar();
    }
    
    private void UpdateStar()
    {
        inkRatio = levelCompleteBar.fillAmount;
        if (inkRatio is < 2f / 3f and > 1f / 3f)
        {
            _levelManager.CurrentLevel.SetStarsCount(2);
            starsImages[2].sprite = loseStarSprite;
        } else if (inkRatio is < 1f / 3f and > 0)
        {
            _levelManager.CurrentLevel.SetStarsCount(1);
            starsImages[1].sprite = loseStarSprite;
        }
    }
    
    public void UpdateLevelText(int level)
    {
        levelText.text = "Level " + level;
    }
    
    public void BackButtonClick()
    {
        _levelManager.StateIndex++;

        _uiManager.OpenUI<UIMainMenu>();
        _gameManager.ChangeState(GameState.MainMenu);

        _levelManager.DespawnLevel();
        _linesDrawer.OnLoadNewLevelOrUI();

        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);

        CloseImmediately();
    }

    public void RetryButtonClick()
    {
        _levelManager.StateIndex++;

        Debug.Log("Retry");
        if (!_gameManager.IsState(GameState.GamePlay))
        {
            _gameManager.ChangeState(GameState.GamePlay);
        }
        _gameManager.StopTween();
        _levelManager.RetryLevel();

        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);

        Init();
    }
    
    public void ResetTickWin()
    {
        tickWin.rectTransform.localScale = Vector3.one;
        tickWin.gameObject.SetActive(false);
    }
    
    public void ResetTickLose()
    {
        tickLose.rectTransform.localScale = Vector3.one;
        tickLose.gameObject.SetActive(false);
    }
}