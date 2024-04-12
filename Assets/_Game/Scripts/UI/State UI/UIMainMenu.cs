using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

public class UIMainMenu : UICanvas
{
    [SerializeField] private Text coinText;
    [SerializeField] private RectTransform playButtonRect, listButtonRect, shopButtonRect, settingButtonRect, coinRect, titleRect;
    
    private Tweener scaleTween;
    private GameManager _gameManager;
    private LevelManager _levelManager;
    private DataController _dataController;
    private AudioManager _audioManager;

    [Inject]
    private void Construct (GameManager gameManager, LevelManager levelManager, DataController dataController, AudioManager audioManager)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;
        _dataController = dataController;
        _audioManager = audioManager;
    }
    
    private void OnEnable()
    {
        //SetAnimationForMainMenu();
        UpdateCoinText();
        //UbiAdsManager.Instance.ShowBanner(true);
    }

    private void OnDisable()
    {
        //ResetAnimation();

    }

    public void UpdateCoinText()
    {
        coinText.text = _dataController.currentGameData.coin.ToString();
    }

    private void SetAnimationForMainMenu()
    {
        playButtonRect.DOAnchorPos(new Vector2(0f, -527f), 0.8f).SetEase(Ease.OutSine).SetLoops(0);
        listButtonRect.DOAnchorPos(new Vector2(-369f, -348f), 0.8f).SetEase(Ease.OutSine).SetLoops(0);
        shopButtonRect.DOAnchorPos(new Vector2(76.5f, -189f), 0.4f).SetEase(Ease.OutSine);
        settingButtonRect.DOAnchorPos(new Vector2(-496f, 920f), 0.4f).SetEase(Ease.OutSine);
        coinRect.DOAnchorPos(new Vector2(-200f, -110f), 0.4f).SetEase(Ease.OutSine);

        titleRect.localScale = Vector3.one;
        scaleTween = titleRect.DOScale(new Vector3(1.35f, 1.35f, 1f), 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
    private void ResetAnimation()
    {
        playButtonRect.DOAnchorPos(new Vector2(0f, -1337f), 0.4f).SetEase(Ease.Linear).SetLoops(0);
        listButtonRect.DOAnchorPos(new Vector2(-841f, -378f), 0.8f).SetEase(Ease.OutSine).SetLoops(0);
        shopButtonRect.DOAnchorPos(new Vector2(400f, -189f), 0.4f).SetEase(Ease.OutSine);
        settingButtonRect.DOAnchorPos(new Vector2(-496f, 1132f), 0.4f).SetEase(Ease.OutSine);
        coinRect.DOAnchorPos(new Vector2(-200f, 112f), 0.4f).SetEase(Ease.OutSine);
        scaleTween?.Kill();
    }

    public void PlayButton()
    {
       
        _gameManager.ChangeState(GameState.GamePlay);
        _audioManager.Play(Constant.AUDIO_SFX_PLAY);

        _uiManager.OpenUI<UIGameplay>();
        // Load level
        int level = _dataController.currentGameData.currentLevelInProgress;
        _levelManager.OnLoadLevel(level);
        

        // UI

        CloseDirectly();
    }
    public void SettingButton()
    {
       
        _uiManager.OpenUI<UISettings>();
        CloseDirectly();

        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
    }
    public void SkinShopButton()
    {
       
        _uiManager.OpenUI<UIShop>();

        CloseDirectly();

        _audioManager.PlayBGM(Constant.AUDIO_MUSIC_SHOP);
        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
    }
    public void ListActButton()
    {
       
        _uiManager.OpenUI<UiListAct>();
        CloseDirectly();

        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
    }
    public void ClaimButton()
    {
       
        _uiManager.OpenUI<UIDailyReward>();

        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
    }
    public void ChangeCoinTest()
    {
        _dataController.currentGameData.coin += 100;
        UpdateCoinText();
    }
}
