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

    [Inject]
    private void Construct(GameManager gameManager, LevelManager levelManager, DataPersistence dataPersistence)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;
        _dataPersistence = dataPersistence;
    }
    
    
    private void OnEnable()
    {

        OnInit();
        HandleAudioInto();
        UpdateCoinText();
        SetStarForLevel();
        SetColorStarWhenWin();
        UpdateCoinRewardText();
       
        DisplayChar(SkinController.Instance.currentSkinIndex);
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
        if (_levelManager.CurrentLevelIndex < DataController.Instance.currentGameData.currentLevelInProgress ||
            DataController.Instance.currentGameData.starDoneInLevels[_levelManager.CurrentLevelIndex] != 0)
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
        coinText.text = DataController.Instance.currentGameData.coin.ToString();
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
            if (!UIManager.Instance.IsLoaded<UiListAct>())
            {
                UIManager.Instance.OpenUI<UiListAct>();
                UIManager.Instance.GetUI<UiListAct>().ClearListAct();
                UIManager.Instance.CloseUI<UiListAct>();
            }
            if (act >= UIManager.Instance.GetUI<UiListAct>().actSOList.Count)
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
        for (int i = 0; i < DataController.Instance.currentGameData.starDoneInLevels.Count; i++)
        {
            totalStar += DataController.Instance.currentGameData.starDoneInLevels[i];
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
        UIManager.Instance.CloseUI<UIWin>();
        UIManager.Instance.OpenUI<UIGameplay>();
        UIManager.Instance.GetUI<UIGameplay>().OnInit();
        _levelManager.OnLoadNextLevel();
        _gameManager.ChangeState(GameState.GamePlay);

        AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);


        LinesDrawer.instance.HideLineCantDraw();

        _dataPersistence.SaveGame();
    }



    public void RetryButton()
    {
        UIManager.Instance.CloseUI<UIWin>();
        UIManager.Instance.OpenUI<UIGameplay>();
        UIManager.Instance.GetUI<UIGameplay>().OnInit();
        _levelManager.OnRetry();
        _gameManager.ChangeState(GameState.GamePlay);

        AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);


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
        if (DataController.Instance.currentGameData.starDoneInLevels.Count == 0)
        {
            for (int i = 0; i < 999; i++)
            {
                DataController.Instance.currentGameData.starDoneInLevels.Add(0);
            }
        }
    }
    public void SetStarForLevel()
    {
        if (DataController.Instance.currentGameData.starDoneInLevels.Count == 0)
        {
            for (int i = 0; i < 999; i++)
            {
                DataController.Instance.currentGameData.starDoneInLevels.Add(0);
            }
        }
        star = _levelManager.currentLevel.star;
        if (DataController.Instance.currentGameData.starDoneInLevels[_levelManager.currentLevel.levelNumberInGame] < star)
        {
            DataController.Instance.currentGameData.starDoneInLevels[_levelManager.currentLevel.levelNumberInGame] = star;
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
        AudioManager.instance.PauseBGM();
        AudioManager.instance.Play(Constant.AUDIO_SFX_WIN);
    }
    private void HandleAudioOut()
    {
        AudioManager.instance.UnPauseBGM();
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
