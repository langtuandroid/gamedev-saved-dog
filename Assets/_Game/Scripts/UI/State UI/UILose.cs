using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine;
using Spine.Unity;
using TMPro;
using Zenject;

public class UILose : UICanvas
{
    [SerializeField] private RectTransform PopupLose;
    [SerializeField] private RectTransform dogImage1;
    [SerializeField] private RectTransform dogImage2;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private SkeletonGraphic dogSkeletonAnimation1;
    [SerializeField] private SkeletonGraphic dogSkeletonAnimation2;

    private SkeletonData skeletonData = new SkeletonData();
    private List<Skin> skins;
    
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
        UpdateCoinText();
        HandleAudio();

        DisplayCharacter(_skinController.CurrentSkinIndex);
        SetAnimationLose();
    }
    private void OnDisable()
    {
        HandleAudioOut();
        ResetAnimation();
    }

    private void UpdateCoinText()
    {
        coinText.text = _dataController.currentGameData.coin.ToString();
    }
    
    private void HandleAudio()
    {
        _audioManager.PauseBGM();
        _audioManager.Play(Constant.AUDIO_SFX_LOSE_UI);
    }
    
    private void HandleAudioOut()
    {
        _audioManager.UnPauseBGM();
    }
    
    public void RetryButtonClick()
    {
        StartCoroutine(Retry());
    }

    private IEnumerator Retry()
    {
        yield return new WaitForSeconds(1);
        _uiManager.CloseUI<UILose>();
        _uiManager.OpenUI<UIGameplay>();
        _uiManager.GetUI<UIGameplay>().Init();
        _levelManager.RetryLevel();
        _gameManager.ChangeState(GameState.GamePlay);

        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);

        _dataPersistence.SaveGame();
    }
    
    public void MenuButtonClick()
    {
        if (_levelManager.CurrentLevel.LevelNumberInGame > 1)
        {
            StartCoroutine(Home());
        } else
        {
            _levelManager.StateIndex++;

            _uiManager.OpenUI<UIMainMenu>();
            _gameManager.ChangeState(GameState.MainMenu);

            _levelManager.DespawnLevel();
            _linesDrawer.OnLoadNewLevelOrUI();

            _audioManager.Play(Constant.AUDIO_SFX_BUTTON);

            CloseImmediately();
        }
    }

    private IEnumerator Home()
    {
        yield return new WaitForSeconds(1);
        _levelManager.StateIndex++;

        _uiManager.OpenUI<UIMainMenu>();
        _gameManager.ChangeState(GameState.MainMenu);

        _levelManager.DespawnLevel();
        _linesDrawer.OnLoadNewLevelOrUI();

        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);

        CloseImmediately();
        
    }

    public void ShopButtonClick()
    {
        if (_levelManager.CurrentLevel.LevelNumberInGame > 1)
        {
            StartCoroutine(Shop());
        } else
        {
            _uiManager.OpenUI<UIShop>();
            _gameManager.ChangeState(GameState.MainMenu);

            _levelManager.DespawnLevel();
            _linesDrawer.OnLoadNewLevelOrUI();

            CloseImmediately();

            _audioManager.PlayBGM(Constant.AUDIO_MUSIC_SHOP);
            _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
        }
    }

    private IEnumerator Shop()
    {
        yield return new WaitForSeconds(1);

        _uiManager.OpenUI<UIShop>();
        _gameManager.ChangeState(GameState.MainMenu);

        _levelManager.DespawnLevel();
        _linesDrawer.OnLoadNewLevelOrUI();

        CloseImmediately();

        _audioManager.PlayBGM(Constant.AUDIO_MUSIC_SHOP);
        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
    }
    
    private void DisplayCharacter(int index)
    {
        SetDogeSkin(index, dogSkeletonAnimation1);
        SetDogeSkin(index, dogSkeletonAnimation2);
    }

    private void SetDogeSkin(int skinIndex, SkeletonGraphic skeAnim)
    {
        Constant constant = new Constant();
        
        skeletonData = skeAnim.SkeletonData;
        skins = new List<Skin>(skeletonData.Skins.ToArray());
        Skin skin = skins[constant.skins[skinIndex]];

        skeAnim.Skeleton.SetSkin(skin);
        skeAnim.Skeleton.SetSlotsToSetupPose();
    }

    private void SetAnimationLose()
    {
        PopupLose.DOAnchorPos(new Vector3(0, -196.87f, 0), 0.5f).SetEase(Ease.OutBack);

        dogImage1.DOPunchRotation(new Vector3(0, 180f, 0), 0.5f, 10, 1f).SetEase(Ease.OutQuad);
        dogImage2.DOPunchRotation(new Vector3(0, -180f, 0), 0.5f, 10, 1f).SetEase(Ease.OutQuad);
    }
    
    private void ResetAnimation()
    {
        PopupLose.DOAnchorPos(new Vector3(0, -1230f, 0), 0f);
    }
}
