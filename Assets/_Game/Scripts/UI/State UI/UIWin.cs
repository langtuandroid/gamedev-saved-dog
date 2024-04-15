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
    [SerializeField] private Color starGain, starLose;
    [SerializeField] private Image star2, star3, popup;
    public Text coinText, coinRewardText, coinAdText;
    [SerializeField] private CoinReward coinReward;
    [SerializeField] private GameObject coverCoinGain;
    [SerializeField] private Text popupText;
    [SerializeField] private SkeletonGraphic skeletonAnimation1, skeletonAnimation2;
    private Tweener scaleTween;
    private int star;
    private bool onceClick;
    public int amount;

    // Render Skin
    private SkeletonData skeletonData = new SkeletonData();
    private List<Skin> skins;
    private Skin currentSkin;
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
        SetStarForLevel();
        SetColorStarWhenWin();
        UpdateCoinRewardText();
       
        DisplayChar(_skinController.CurrentSkinIndex);
        SetAnimationForUIWin();

       
        
        
    }
    private void OnDisable()
    {
        HandleAudioOut();
        ResetAnimation();
        SetColorStarDefault();
        SetDefaultPopup();
    }
    private new void OnInit()
    {
        CheckStarInLevels();
        onceClick = true;
        amount = 10;
        //Debug.Log(DataController.Instance.currentGameData.starDoneInLevels[LevelManager.Instance.CurrentLevelIndex] + "--"
        //    + LevelManager.Instance.CurrentLevelIndex);
        // Check level done, if done hide coin reward 
        //Debug.Log(LevelManager.Instance.CurrentLevelIndex + " va " + DataController.Instance.currentGameData.currentLevelInProgress);
        if (_levelManager.CurrentLevelIndex < _dataController.currentGameData.currentLevelInProgress ||
            _dataController.currentGameData.starDoneInLevels[_levelManager.CurrentLevelIndex] != 0)
        {
            amount = 0;
            coverCoinGain.SetActive(false);
        }
        else
        {
            coverCoinGain.SetActive(true);
        }
    }

    public void UpdateCoinText()
    {
        coinText.text = _dataController.currentGameData.coin.ToString();
    }
    public void UpdateCoinRewardText()
    {
        if (amount == 0)
            return;

        amount += 5 * star;
        coinRewardText.text = amount.ToString();
    }
   
    private void DisplayChar(int index)
    {
        SetSkin(index, skeletonAnimation1);
        SetSkin(index, skeletonAnimation2);
    }

    private void SetSkin(int skinIndex, SkeletonGraphic skeAnim)
    {
        Constant constant = new Constant();

        // Setup Skin
        skeletonData = skeAnim.SkeletonData;
        skins = new List<Skin>(skeletonData.Skins.ToArray());
        Skin skin = skins[constant.skins[skinIndex]];

        skeAnim.Skeleton.SetSkin(skin);
        skeAnim.Skeleton.SetSlotsToSetupPose();
    }


    public void AdButton()
    {
        if (onceClick == false)
            return;
        onceClick = false;

        }

    public void ReplayRewardButton()
    {
        if (onceClick == false)
            return;
        onceClick = false;
    }

    public void NextLevelButton()
    {
        // check level next act
        if ((_levelManager.CurrentLevelIndex + 1) % 10 == 0)
        {
            int act = (_levelManager.CurrentLevelIndex + 1) / 10;

            // not enough star
            if (!_uiManager.IsLoaded<UiListAct>())
            {
                _uiManager.OpenUI<UiListAct>();
                _uiManager.GetUI<UiListAct>().ClearListAct();
                _uiManager.CloseUI<UiListAct>();
            }
            if (act >= _uiManager.GetUI<UiListAct>().actSOList.Count)
            {
                HandleNextLevel();
                return;
            }
            //if (UIManager.Instance.GetUI<UiListAct>().actSOList[act].starUnlock > GetTotalStarInGame())
            //{
            //    SetAnimPopup();
            //    return;
            //}
        }

        if (onceClick == false)
            return;
        onceClick = false;


        HandleNextLevel();
        //coinReward.AnimCoinFly(amount);
    }
    private void SetAnimPopup()
    {
        popup.gameObject.SetActive(true);

        popup.DOFade(0f, 0f);
        popupText.DOFade(0f, 0f);

        popup.DOFade(1f, 1.7f).SetEase(Ease.InOutSine);
        popupText.DOFade(1f, 1.7f).SetEase(Ease.InOutSine);

        popup.DOFade(0f, 1f).SetDelay(2.5f).SetEase(Ease.InOutSine);
        popupText.DOFade(0f, 1f).SetDelay(2.5f).SetEase(Ease.InOutSine).OnComplete(SetDefaultPopup);
    }
    private void SetDefaultPopup()
    {
        popup.DOFade(0f, 1f).SetEase(Ease.InOutSine);
        popupText.DOFade(0f, 1f).SetEase(Ease.InOutSine);
        popup.gameObject.SetActive(false);
    }

    private int GetTotalStarInGame()
    {
        int totalStar = 0;
        for (int i = 0; i < _dataController.currentGameData.starDoneInLevels.Count; i++)
        {
            totalStar += _dataController.currentGameData.starDoneInLevels[i];
        }
        return totalStar;
    }
    public void HandleNextLevel()
    {
        //UIManager.Instance.CloseUI<UIWin>();
        //UIManager.Instance.OpenUI<UIGameplay>();
        //UIManager.Instance.GetUI<UIGameplay>().OnInit();
        //LevelManager.Instance.OnLoadNextLevel();
        //GameManager.Instance.ChangeState(GameState.GamePlay);

        //AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);
        //UbiAdsManager.Instance.ShowInterstitial(()=> { }, () => { }, true);

        //LinesDrawer.instance.HideLineCantDraw();

        //DataPersistence.Instance.SaveGame();

        StartCoroutine(iHandleNextLevel());
    }

    IEnumerator iHandleNextLevel()
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
    public void SetAnimationForUIWin()
    {
        
        dog1.DOPunchRotation(new Vector3(0, 180f, 0), 0.5f, 10, 1f).SetEase(Ease.OutQuad);
        dog2.DOPunchRotation(new Vector3(0, -180f, 0), 0.5f, 10, 1f).SetEase(Ease.OutQuad);

        nextButton.DOAnchorPos(new Vector2(0, -606f), 0.5f).SetEase(Ease.InOutSine);
        retryButton.DOAnchorPos(new Vector2(0, -831f), 0.5f).SetEase(Ease.InOutSine);
       

        coinReward.OnInit();
        coinReward.AnimCoinFly(amount, false);
    }
    public void ResetAnimation()
    {
        nextButton.DOAnchorPos(new Vector2(-784f, -606f), 0f).SetEase(Ease.InOutSine);
        retryButton.DOAnchorPos(new Vector2(767f, -606f), 0f).SetEase(Ease.InOutSine);
         scaleTween?.Kill();
    }
    private void CheckStarInLevels()
    {
        if (_dataController.currentGameData.starDoneInLevels.Count == 0)
        {
            for (int i = 0; i < 999; i++)
            {
                _dataController.currentGameData.starDoneInLevels.Add(0);
            }
        }
    }
    public void SetStarForLevel()
    {
        if (_dataController.currentGameData.starDoneInLevels.Count == 0)
        {
            for (int i = 0; i < 999; i++)
            {
                _dataController.currentGameData.starDoneInLevels.Add(0);
            }
        }
        star = _levelManager.currentLevel.StarsCount;
        if (_dataController.currentGameData.starDoneInLevels[_levelManager.currentLevel.LevelNumberInGame] < star)
        {
            _dataController.currentGameData.starDoneInLevels[_levelManager.currentLevel.LevelNumberInGame] = star;
        }
    }
    public void SetColorStarWhenWin()
    {
        if (star == 1)
        {
            star2.color = starLose;
            star3.color = starLose;
        }
        else if (star == 2)
        {
            star3.color = starLose;
        }
    }
    public void SetColorStarDefault()
    {
        star2.color = starGain;
        star3.color = starGain;
    }
    private void HandleAudioInto()
    {
        _audioManager.PauseBGM();
        _audioManager.Play(Constant.AUDIO_SFX_WIN);
    }
    private void HandleAudioOut()
    {
        _audioManager.UnPauseBGM();
    }

    #region Ad
    void OnReplayReward()
    {
        coinReward.AnimCoinFly(10, true);
    }

    void OnSuccess()
    {
        coinReward.AnimCoinFly(amount, true);
    }

    void OnFailed()
    {
        Debug.Log("Show Video Failed");
    }
    #endregion
}
