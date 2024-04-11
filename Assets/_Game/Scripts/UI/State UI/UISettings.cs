using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UISettings : UICanvas
{
    [SerializeField] private Image musicButtonIcon, soundButtonIcon, vibrateButtonIcon;
    [SerializeField] private Sprite musicOn, musicOff, soundOn, soundOff, vibrateOn, vibrateOff;
    [SerializeField] private RectTransform boxRect;
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
        musicButtonIcon.sprite = (DataController.Instance.currentGameData.music == false) ? musicOn : musicOff;
        soundButtonIcon.sprite = (DataController.Instance.currentGameData.sound == false) ? soundOn : soundOff;
        vibrateButtonIcon.sprite = (DataController.Instance.currentGameData.vibrate == false) ? vibrateOn : vibrateOff;
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
        musicButtonIcon.sprite = (DataController.Instance.currentGameData.music == true) ? musicOn : musicOff;
        if (DataController.Instance.currentGameData.music == true)
        {
            AudioManager.instance.MusicOn();
        }
        else
        {
            AudioManager.instance.MusicOff();
        }
        DataController.Instance.currentGameData.music = !DataController.Instance.currentGameData.music;

        DataPersistence.Instance.SaveGame();
    }
    public void SoundButton()
    {
        AudioManager.instance.Play(Constant.AUDIO_SFX_BTNSETTINGS);
        soundButtonIcon.sprite = (DataController.Instance.currentGameData.sound == true) ? soundOn : soundOff;
        if (DataController.Instance.currentGameData.sound == true)
        {
            AudioManager.instance.SoundOn();
        }
        else
        {
            AudioManager.instance.SoundOff();
        }
        DataController.Instance.currentGameData.sound = !DataController.Instance.currentGameData.sound;

        DataPersistence.Instance.SaveGame();
    }
    public void VibrateButton()
    {
        AudioManager.instance.Play(Constant.AUDIO_SFX_BTNSETTINGS);
        vibrateButtonIcon.sprite = (DataController.Instance.currentGameData.vibrate == true) ? vibrateOn : vibrateOff;
        DataController.Instance.currentGameData.vibrate = !DataController.Instance.currentGameData.vibrate;

        DataPersistence.Instance.SaveGame();
    }
}
