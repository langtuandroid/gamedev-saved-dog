using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

public class UIGameplay : UICanvas
{
    [Header("Ink Section")]
    public Image inkBar;
    public int star;

    [SerializeField] private RectTransform coinImage;
    [SerializeField] private GameObject panelTut;
    [SerializeField] private Transform clockCover;
    [SerializeField] private Image[] stars;
    [SerializeField] private Color colorLoseStar;

    [SerializeField] private float maxInk; public float MaxInk { get { return maxInk; } }
    [SerializeField] private float inkLoseRate; public float InkLoseRate { get { return inkLoseRate; } }
    private float currentInk; public float CurrentInk { get { return currentInk; } set { currentInk = value; } }
    private float inkRatio;

    private Tween tween1, tween2, tween3;
    private Vector3 initPosClock;

    private int startValue, endValue;

    [Header("TextLevel Section")]
    [SerializeField] private Text levelText, coinText;

    public Image tickWin, tickLose;
    private GameManager _gameManager;

    [Inject]
    private void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    private void Awake()
    {
        initPosClock = clockCover.position;
    }
    void OnEnable()
    {
        OnInit();

        LinesDrawer.instance.OnEndDraw += AnimClock;
    }
    private void OnDisable()
    {
        LinesDrawer.instance.OnEndDraw -= AnimClock;

        ResetAnimClock();
    }

    public new void OnInit()
    {
        star = 3;
        inkBar.fillAmount = 1f;
        currentInk = maxInk;
        ColorStarReturnDefault();
        ResetTickWin();
        ResetTickLose();
        UpdateCoinText();
        ResetAnimClock();
        ResetCoinImage();
    }

    public void HandleCoinGainInGameplay(int loop)
    {
        coinImage.DOScale(1.2f, 0.1f).SetEase(Ease.Linear).SetLoops(loop, LoopType.Yoyo);

        startValue = DataController.Instance.currentGameData.coin;
        endValue = DataController.Instance.currentGameData.coin + loop/2;
        DataController.Instance.currentGameData.coin += loop / 2;
        coinText.DOText(endValue.ToString(), 0.5f).OnComplete(ResetCoinImage);
    }
    public void ResetCoinImage()
    {
        coinImage.DOScale(1f, 0f);
    }
    private void AnimClock()
    {
        tween1 = clockCover.DOShakePosition(1f, 3f, 26, 90f, false).SetEase(Ease.InQuart).SetLoops(-1, LoopType.Yoyo);
        tween2 = clockCover.DOShakeRotation(1f, 10f, 10, 90f).SetEase(Ease.InQuart).SetLoops(-1, LoopType.Yoyo);
        tween3 = clockCover.DOShakeScale(1, 0.2f, 10, 90f).SetEase(Ease.InQuart).SetLoops(-1, LoopType.Yoyo);
    }

    private void ResetAnimClock()
    {
        tween1.Kill();
        tween2.Kill();
        tween3.Kill();

        clockCover.transform.position = initPosClock;
        clockCover.transform.localRotation = Quaternion.identity;
        clockCover.transform.localScale = Vector3.one;
    }

    private void UpdateCoinText()
    {
        coinText.text = DataController.Instance.currentGameData.coin.ToString();
    }
    private void ColorStarReturnDefault()
    {
        stars[1].color = stars[0].color;
        stars[2].color = stars[0].color;
    }

    public void UpdateInkBar()
    {
        inkBar.fillAmount = currentInk / maxInk;
        UpdateStar();
    }
    private void UpdateStar()
    {
        inkRatio = inkBar.fillAmount;
        if (inkRatio < 2f / 3f && inkRatio > 1f / 3f)
        {
            star = 2;
            LevelManager.Instance.currentLevel.star = 2;
            stars[2].color = colorLoseStar;
        }
        else if (inkRatio < 1f / 3f && inkRatio > 0)
        {
            star = 1;
            LevelManager.Instance.currentLevel.star = 1;
            stars[1].color = colorLoseStar;
        }
    }
    public void UpdateLevelText(int level)
    {
        levelText.text = "Level " + level;
    }
    public void BackButton()
    {
        
        
            OnBackButton();
        
        
    }
    
    void OnBackButton()
    {
        LevelManager.Instance.stateIndex++;

        UIManager.Instance.OpenUI<UIMainMenu>();
        _gameManager.ChangeState(GameState.MainMenu);

        LevelManager.Instance.Despawn();
        LinesDrawer.instance.OnLoadNewLevelOrUI();

        AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);

        CloseDirectly();
    }

    public void RetryButton()
    {
          OnRetryButton();
    }
    public void OnRetryButton()
    {
        LevelManager.Instance.stateIndex++;

        Debug.Log("Retry");
        if (!_gameManager.IsState(GameState.GamePlay))
        {
            _gameManager.ChangeState(GameState.GamePlay);
        }
        _gameManager.StopTween();
        LevelManager.Instance.OnRetry();

        AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);

        OnInit();
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
    public void ShowTut()
    {
        //panelTut.SetActive(true);
        //TimeManager.Instance.SlowTime();
        //Transform handAttack = LevelManager.Instance.currentLevel.transform.Find("Tut Hand Attack");
        //handAttack.gameObject.SetActive(true);
    }
    public void TryButton()
    {
        
    }
}