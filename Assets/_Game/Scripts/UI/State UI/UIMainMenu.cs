using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIMainMenu : UICanvas
{
    [SerializeField] private Text coinText;
    [SerializeField] private RectTransform playButtonRect, listButtonRect, shopButtonRect, settingButtonRect, coinRect, titleRect;
    private Tweener scaleTween;

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
        coinText.text = DataController.Instance.currentGameData.coin.ToString();
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
       
        GameManager.Instance.ChangeState(GameState.GamePlay);
        AudioManager.instance.Play(Constant.AUDIO_SFX_PLAY);

        UIManager.Instance.OpenUI<UIGameplay>();
        // Load level
        int level = DataController.Instance.currentGameData.currentLevelInProgress;
        LevelManager.Instance.OnLoadLevel(level);
        

        // UI

        CloseDirectly();
    }
    public void SettingButton()
    {
       
        UIManager.Instance.OpenUI<UISettings>();
        CloseDirectly();

        AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);
    }
    public void SkinShopButton()
    {
       
        UIManager.Instance.OpenUI<UIShop>();

        CloseDirectly();

        AudioManager.instance.PlayBGM(Constant.AUDIO_MUSIC_SHOP);
        AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);
    }
    public void ListActButton()
    {
       
        UIManager.Instance.OpenUI<UiListAct>();
        CloseDirectly();

        AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);
    }
    public void ClaimButton()
    {
       
        UIManager.Instance.OpenUI<UIDailyReward>();

        AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);
    }
    public void ChangeCoinTest()
    {
        DataController.Instance.currentGameData.coin += 100;
        UpdateCoinText();
    }
}
