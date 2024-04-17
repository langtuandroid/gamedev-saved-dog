using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

public class UISettings : UICanvas
{
    private const string TERMS_OF_USE = "https://www.google.com.ua/?hl=uk";
    private const string PRIVACY_POLICY = "https://www.google.com.ua/?hl=uk";
    
    [SerializeField] private Image musicButtonIcon, soundButtonIcon, vibrateButtonIcon;
    [SerializeField] private Sprite musicOn, musicOff, soundOn, soundOff, vibrateOn, vibrateOff;
    [SerializeField] private RectTransform boxRect;

    [Header("Buttons")]
    [SerializeField]
    private Button privacyPolicy;
    [SerializeField]
    private Button termOfUse;

    private DataPersistence _dataPersistence;
    private DataController _dataController;
    private AudioManager _audioManager;

    [Inject]
    private void Construct(DataPersistence dataPersistence, DataController dataController, AudioManager audioManager)
    {
        _dataPersistence = dataPersistence;
        _dataController = dataController;
        _audioManager = audioManager;
    }

    private void Awake()
    {
        privacyPolicy.onClick.AddListener((() => { Application.OpenURL(PRIVACY_POLICY);}));
        termOfUse.onClick.AddListener((() => { Application.OpenURL(TERMS_OF_USE);}));
    }

    private void OnEnable()
    {
        //SetAnimationForSetting();
        LoadUISetting();
    }

    private void OnDestroy()
    {
        privacyPolicy.onClick.RemoveAllListeners();
        termOfUse.onClick.RemoveAllListeners();
    }

    private void SetAnimationForSetting()
    {
        boxRect.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        boxRect.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutSine);
    }

    private void LoadUISetting()
    {
        musicButtonIcon.sprite = (_dataController.currentGameData.music == false) ? musicOn : musicOff;
        soundButtonIcon.sprite = (_dataController.currentGameData.sound == false) ? soundOn : soundOff;
        vibrateButtonIcon.sprite = (_dataController.currentGameData.vibrate == false) ? vibrateOn : vibrateOff;
    }

    public void BackButton()
    {
        _uiManager.OpenUI<UIMainMenu>();
        CloseImmediately();

        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
    }
    public void MusicButton()
    {
        _audioManager.Play(Constant.AUDIO_SFX_BTNSETTINGS);
        musicButtonIcon.sprite = (_dataController.currentGameData.music == true) ? musicOn : musicOff;
        if (_dataController.currentGameData.music == true)
        {
            _audioManager.MusicOn();
        } else
        {
            _audioManager.MusicOff();
        }
        _dataController.currentGameData.music = !_dataController.currentGameData.music;

        _dataPersistence.SaveGame();
    }
    public void SoundButton()
    {
        _audioManager.Play(Constant.AUDIO_SFX_BTNSETTINGS);
        soundButtonIcon.sprite = (_dataController.currentGameData.sound == true) ? soundOn : soundOff;
        if (_dataController.currentGameData.sound == true)
        {
            _audioManager.SoundOn();
        } else
        {
            _audioManager.SoundOff();
        }
        _dataController.currentGameData.sound = !_dataController.currentGameData.sound;

        _dataPersistence.SaveGame();
    }
    public void VibrateButton()
    {
        _audioManager.Play(Constant.AUDIO_SFX_BTNSETTINGS);
        vibrateButtonIcon.sprite = (_dataController.currentGameData.vibrate == true) ? vibrateOn : vibrateOff;
        _dataController.currentGameData.vibrate = !_dataController.currentGameData.vibrate;

        _dataPersistence.SaveGame();
    }
}
