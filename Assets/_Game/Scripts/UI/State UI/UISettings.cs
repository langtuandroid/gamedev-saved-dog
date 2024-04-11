using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

public class UISettings : UICanvas
{
    [SerializeField] private Image musicButtonIcon, soundButtonIcon, vibrateButtonIcon;
    [SerializeField] private Sprite musicOn, musicOff, soundOn, soundOff, vibrateOn, vibrateOff;
    [SerializeField] private RectTransform boxRect;

    private DataPersistence _dataPersistence;
    private DataController _dataController;

    [Inject]
    private void Construct(DataPersistence dataPersistence, DataController dataController)
    {
        _dataPersistence = dataPersistence;
        _dataController = dataController;
    }
    
    private void OnEnable()
    {
        SetAnimationForSetting();
        LoadUISetting();
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
        UIManager.Instance.OpenUI<UIMainMenu>();
        CloseDirectly();

        AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);
    }
    public void MusicButton()
    {
        AudioManager.instance.Play(Constant.AUDIO_SFX_BTNSETTINGS);
        musicButtonIcon.sprite = (_dataController.currentGameData.music == true) ? musicOn : musicOff;
        if (_dataController.currentGameData.music == true)
        {
            AudioManager.instance.MusicOn();
        }
        else
        {
            AudioManager.instance.MusicOff();
        }
        _dataController.currentGameData.music = !_dataController.currentGameData.music;

        _dataPersistence.SaveGame();
    }
    public void SoundButton()
    {
        AudioManager.instance.Play(Constant.AUDIO_SFX_BTNSETTINGS);
        soundButtonIcon.sprite = (_dataController.currentGameData.sound == true) ? soundOn : soundOff;
        if (_dataController.currentGameData.sound == true)
        {
            AudioManager.instance.SoundOn();
        }
        else
        {
            AudioManager.instance.SoundOff();
        }
        _dataController.currentGameData.sound = !_dataController.currentGameData.sound;

        _dataPersistence.SaveGame();
    }
    public void VibrateButton()
    {
        AudioManager.instance.Play(Constant.AUDIO_SFX_BTNSETTINGS);
        vibrateButtonIcon.sprite = (_dataController.currentGameData.vibrate == true) ? vibrateOn : vibrateOff;
        _dataController.currentGameData.vibrate = !_dataController.currentGameData.vibrate;

        _dataPersistence.SaveGame();
    }
}
