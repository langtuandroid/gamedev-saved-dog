using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using Zenject;

public class UIWin : UICanvas
{
    public RectTransform nextButton, retryButton, dog1, dog2;
    public Text coinText, coinRewardText;
    [SerializeField] private Color starGain, starLose;
    [SerializeField] private Image star2, star3, popup;
    [SerializeField] private CoinReward coinReward;
    [SerializeField] private GameObject coverCoinGain;
    [SerializeField] private Text popupText;
    [SerializeField] private SkeletonGraphic skeletonAnimation1, skeletonAnimation2;

    private Tweener _scaleTween;
    private int _starsCount;
    private bool _isClicked;
    private int _coinsAmount;
    
    private SkeletonData _skeletonData = new SkeletonData();
    private List<Skin> _skinsList;
    private Skin _currentSkin;
    private GameManager _gameManager;
    private LevelManager _levelManager;
    private DataPersistence _dataPersistence;
    private DataController _dataController;
    private AudioManager _audioManager;
    private SkinController _skinController;
    private LinesDrawer _linesDrawer;

    [Inject]
    private void Construct(
        GameManager gameManager, LevelManager levelManager, DataPersistence dataPersistence, DataController dataController, AudioManager audioManager, 
         SkinController skinController, LinesDrawer linesDrawer)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;
        _dataPersistence = dataPersistence;
        _dataController = dataController;
        _audioManager = audioManager;
        _skinController = skinController;
        _linesDrawer = linesDrawer;
    }

    private void OnEnable()
    {
        OnInit();
        HandleAudioInto();
        UpdateCoinText();
        SetStars();
        SetColorStarWhenWin();
        UpdateCoinRewardText();
       
        DisplayChar(_skinController.CurrentSkinIndex);
        SetUIWinAnimation();
    }
    private void OnDisable()
    {
        ResetAnimation();
        SetColorStarDefault();
        SetDefaultPopup();
        
        _audioManager.UnPauseBGM();
    }
    private void OnInit()
    {
        CheckStars();
        _isClicked = true;
        _coinsAmount = 10;
        
        if (_levelManager.CurrentLevelIndex < _dataController.currentGameData.currentLevelInProgress ||
            _dataController.currentGameData.starDoneInLevels[_levelManager.CurrentLevelIndex] != 0)
        {
            _coinsAmount = 0;
            coverCoinGain.SetActive(false);
        } else
        {
            coverCoinGain.SetActive(true);
        }
    }

    private void UpdateCoinText()
    {
        coinText.text = _dataController.currentGameData.coin.ToString();
    }

    private void UpdateCoinRewardText()
    {
        if (_coinsAmount == 0)
            return;

        _coinsAmount += 5 * _starsCount;
        coinRewardText.text = _coinsAmount.ToString();
    }
   
    private void DisplayChar(int index)
    {
        SetDogeSkin(index, skeletonAnimation1);
        SetDogeSkin(index, skeletonAnimation2);
    }

    private void SetDogeSkin(int skinIndex, SkeletonGraphic skeAnim)
    {
        Constant constant = new Constant();
        
        _skeletonData = skeAnim.SkeletonData;
        _skinsList = new List<Skin>(_skeletonData.Skins.ToArray());
        Skin skin = _skinsList[constant.skins[skinIndex]];

        skeAnim.Skeleton.SetSkin(skin);
        skeAnim.Skeleton.SetSlotsToSetupPose();
    }
    
    public void NextLevelButton()
    {
        if ((_levelManager.CurrentLevelIndex + 1) % 10 == 0)
        {
            int act = (_levelManager.CurrentLevelIndex + 1) / 10;
            
            if (!_uiManager.IsLoaded<UiListAct>())
            {
                _uiManager.OpenUI<UiListAct>();
                _uiManager.GetUI<UiListAct>().ClearListAct();
                _uiManager.CloseUI<UiListAct>();
            }
            if (act >= _uiManager.GetUI<UiListAct>().actSOList.Count)
            {
                StartCoroutine(HandleNextLevel());
                return;
            }
        }

        if (!_isClicked)
            return;
        _isClicked = false;
        
        StartCoroutine(HandleNextLevel());
    }

    private void SetDefaultPopup()
    {
        popup.DOFade(0f, 1f).SetEase(Ease.InOutSine);
        popupText.DOFade(0f, 1f).SetEase(Ease.InOutSine);
        popup.gameObject.SetActive(false);
    }

    public IEnumerator HandleNextLevel()
    {
        yield return new WaitForSeconds(1);
        
        _uiManager.CloseUI<UIWin>();
        _uiManager.OpenUI<UIGameplay>();
        _uiManager.GetUI<UIGameplay>().OnInit();
        _levelManager.OnLoadNextLevel();
        _gameManager.ChangeState(GameState.GamePlay);
        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
        _linesDrawer.HideLineCantDraw();
        _dataPersistence.SaveGame();
    }

    public void RetryButton()
    {
        _uiManager.CloseUI<UIWin>();
        _uiManager.OpenUI<UIGameplay>();
        _uiManager.GetUI<UIGameplay>().OnInit();
        _levelManager.OnRetry();
        _gameManager.ChangeState(GameState.GamePlay);
        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
        _dataPersistence.SaveGame();
    }

    private void SetUIWinAnimation()
    {
        
        dog1.DOPunchRotation(new Vector3(0, 180f, 0), 0.5f, 10, 1f).SetEase(Ease.OutQuad);
        dog2.DOPunchRotation(new Vector3(0, -180f, 0), 0.5f, 10, 1f).SetEase(Ease.OutQuad);

        nextButton.DOAnchorPos(new Vector2(0, -606f), 0.5f).SetEase(Ease.InOutSine);
        retryButton.DOAnchorPos(new Vector2(0, -831f), 0.5f).SetEase(Ease.InOutSine);
       

        coinReward.OnInit();
        coinReward.AnimCoinFly(_coinsAmount, false);
    }

    private void ResetAnimation()
    {
        nextButton.DOAnchorPos(new Vector2(-784f, -606f), 0f).SetEase(Ease.InOutSine);
        retryButton.DOAnchorPos(new Vector2(767f, -606f), 0f).SetEase(Ease.InOutSine);
         _scaleTween?.Kill();
    }
    private void CheckStars()
    {
        if (_dataController.currentGameData.starDoneInLevels.Count != 0)
        {
            return;
        }

        for (int i = 0; i < 999; i++)
        {
            _dataController.currentGameData.starDoneInLevels.Add(0);
        }
    }

    private void SetStars()
    {
        if (_dataController.currentGameData.starDoneInLevels.Count == 0)
        {
            for (int i = 0; i < 999; i++)
            {
                _dataController.currentGameData.starDoneInLevels.Add(0);
            }
        }
        _starsCount = _levelManager.currentLevel.StarsCount;
        if (_dataController.currentGameData.starDoneInLevels[_levelManager.currentLevel.LevelNumberInGame] < _starsCount)
        {
            _dataController.currentGameData.starDoneInLevels[_levelManager.currentLevel.LevelNumberInGame] = _starsCount;
        }
    }

    private void SetColorStarWhenWin()
    {
        if (_starsCount == 1)
        {
            star2.color = starLose;
            star3.color = starLose;
        } else if (_starsCount == 2)
        {
            star3.color = starLose;
        }
    }

    private void SetColorStarDefault()
    {
        star2.color = starGain;
        star3.color = starGain;
    }
    
    private void HandleAudioInto()
    {
        _audioManager.PauseBGM();
        _audioManager.Play(Constant.AUDIO_SFX_WIN);
    }

#region Ad
    void OnReplayReward()
    {
        coinReward.AnimCoinFly(10, true);
    }

    void OnSuccess()
    {
        coinReward.AnimCoinFly(_coinsAmount, true);
    }

    void OnFailed()
    {
        Debug.Log("Show Video Failed");
    }
    #endregion
}
