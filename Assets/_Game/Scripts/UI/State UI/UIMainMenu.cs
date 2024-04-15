using UnityEngine;
using DG.Tweening;
using TMPro;
using Zenject;

public class UIMainMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI coinText;

    private Tweener _scaleTween;
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
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        coinText.text = _dataController.currentGameData.coin.ToString();
    }

    public void PlayButton()
    {
        _gameManager.ChangeState(GameState.GamePlay);
        _audioManager.Play(Constant.AUDIO_SFX_PLAY);

        _uiManager.OpenUI<UIGameplay>();
        int level = _dataController.currentGameData.currentLevelInProgress;
        _levelManager.OnLoadLevel(level);
        
        CloseImmediately();
    }
    
    public void SettingButton()
    {
        _uiManager.OpenUI<UISettings>();
        CloseImmediately();

        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
    }
    
    public void SkinShopButton()
    {
        _uiManager.OpenUI<UIShop>();

        CloseImmediately();

        _audioManager.PlayBGM(Constant.AUDIO_MUSIC_SHOP);
        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
    }
    
    public void ListActButton()
    {
        _uiManager.OpenUI<UiListGroup>();
        CloseImmediately();

        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
    }
    
    public void ClaimButton()
    {
        _uiManager.OpenUI<UIDailyReward>();

        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
    }
}
