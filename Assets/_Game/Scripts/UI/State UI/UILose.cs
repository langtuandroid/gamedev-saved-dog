using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine;
using Spine.Unity;
using Zenject;

public class UILose : UICanvas
{
    public RectTransform PopupLose, dog1, dog2;
    public Text coinText;
    
    private bool onceClick;

    // Render Skin
    private SkeletonData skeletonData = new SkeletonData();
    private List<Skin> skins;
    private Skin currentSkin;
    private GameManager _gameManager;
    private LevelManager _levelManager;
    private DataPersistence _dataPersistence;
    private DataController _dataController;

    [Inject]
    private void Construct(GameManager gameManager, LevelManager levelManager, DataPersistence dataPersistence, DataController dataController)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;
        _dataPersistence = dataPersistence;
        _dataController = dataController;
    }

    [SerializeField] private SkeletonGraphic skeletonAnimation1, skeletonAnimation2;
    private void OnEnable()
    {
        OnInit();
        UpdateCoinText();
        HandleAudioInto();

        DisplayChar(SkinController.Instance.currentSkinIndex);
        SetAnimationForUILose();
    }
    private void OnDisable()
    {
        HandleAudioOut();
        ResetAnimation();

    }

    public new void OnInit()
    {
        onceClick = false;
    }
    public void UpdateCoinText()
    {
        coinText.text = _dataController.currentGameData.coin.ToString();
    }
    private void HandleAudioInto()
    {
        AudioManager.instance.PauseBGM();
        AudioManager.instance.Play(Constant.AUDIO_SFX_LOSE_UI);
    }
    private void HandleAudioOut()
    {
        AudioManager.instance.UnPauseBGM();
    }
    public void RetryButton()
    {
        //UIManager.Instance.CloseUI<UILose>();
        //UIManager.Instance.OpenUI<UIGameplay>();
        //UIManager.Instance.GetUI<UIGameplay>().OnInit();
        //LevelManager.Instance.OnRetry();
        //GameManager.Instance.ChangeState(GameState.GamePlay);

        //AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);

        //DataPersistence.Instance.SaveGame();

        //UbiAdsManager.Instance.ShowInterstitial(() => { }, () => { }, true);
        StartCoroutine(iRetry());
    }

    IEnumerator iRetry()
    {
        yield return new WaitForSeconds(1);
        UIManager.Instance.CloseUI<UILose>();
        UIManager.Instance.OpenUI<UIGameplay>();
        UIManager.Instance.GetUI<UIGameplay>().OnInit();
        _levelManager.OnRetry();
        _gameManager.ChangeState(GameState.GamePlay);

        AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);

        _dataPersistence.SaveGame();
    }
    public void HomeButton()
    {
        if (_levelManager.currentLevel.levelNumberInGame > 1)
        {
            StartCoroutine(iHome());
        }
        else
        {
            _levelManager.stateIndex++;

            UIManager.Instance.OpenUI<UIMainMenu>();
            _gameManager.ChangeState(GameState.MainMenu);

            _levelManager.Despawn();
            LinesDrawer.instance.OnLoadNewLevelOrUI();

            AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);

            CloseDirectly();
        }
    }

    IEnumerator iHome()
    {
        yield return new WaitForSeconds(1);
        _levelManager.stateIndex++;

        UIManager.Instance.OpenUI<UIMainMenu>();
        _gameManager.ChangeState(GameState.MainMenu);

        _levelManager.Despawn();
        LinesDrawer.instance.OnLoadNewLevelOrUI();

        AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);

        CloseDirectly();
        
    }

    public void ShopButton()
    {
        if (_levelManager.currentLevel.levelNumberInGame > 1)
        {
            StartCoroutine(iShop());
        }
        else
        {
            UIManager.Instance.OpenUI<UIShop>();
            _gameManager.ChangeState(GameState.MainMenu);

            _levelManager.Despawn();
            LinesDrawer.instance.OnLoadNewLevelOrUI();

            CloseDirectly();

            AudioManager.instance.PlayBGM(Constant.AUDIO_MUSIC_SHOP);
            AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);
        }
    }
    IEnumerator iShop()
    {
        
        yield return new WaitForSeconds(1);

        UIManager.Instance.OpenUI<UIShop>();
        _gameManager.ChangeState(GameState.MainMenu);

        _levelManager.Despawn();
        LinesDrawer.instance.OnLoadNewLevelOrUI();

        CloseDirectly();

        AudioManager.instance.PlayBGM(Constant.AUDIO_MUSIC_SHOP);
        AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);
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
    public void SetAnimationForUILose()
    {
        PopupLose.DOAnchorPos(new Vector3(0, -196.87f, 0), 0.5f).SetEase(Ease.OutBack);

        dog1.DOPunchRotation(new Vector3(0, 180f, 0), 0.5f, 10, 1f).SetEase(Ease.OutQuad);
        dog2.DOPunchRotation(new Vector3(0, -180f, 0), 0.5f, 10, 1f).SetEase(Ease.OutQuad);
    }
    private void ResetAnimation()
    {
        PopupLose.DOAnchorPos(new Vector3(0, -1230f, 0), 0f);
    }
}
