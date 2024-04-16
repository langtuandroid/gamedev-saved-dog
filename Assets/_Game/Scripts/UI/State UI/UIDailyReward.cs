using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;
using Zenject;

public class UIDailyReward : UICanvas
{
    [FormerlySerializedAs("ready"),SerializeField] private Sprite readyFrameSprite;
    [FormerlySerializedAs("wait"),SerializeField] private Sprite waitFrameSprite;
    [FormerlySerializedAs("claimed"),SerializeField] private Sprite claimedFrameSprite;
    [SerializeField] private Button claimButton;
    [SerializeField] private RectTransform dailyRewardPopup;
    [FormerlySerializedAs("coinReward"),SerializeField] private CoinRewardVip coinRewardVip;
    [FormerlySerializedAs("boxDay"),SerializeField] List<Image> dailyImage;
    [FormerlySerializedAs("rewards"),SerializeField] int[] rewardsPrice;

    private DayState dayState;
    private DataController _dataController;
    private DailyReward _dailyReward;

    [Inject]
    private void Construct(DataController dataController, DailyReward dailyReward)
    {
        _dataController = dataController;
        _dailyReward = dailyReward;
    }
    private void OnEnable()
    {
        LoadRewardUI();
        DailyRewardAnimation();
        
        claimButton.interactable = _dailyReward.canClaim;

        coinRewardVip.OnUpdateCoin += UpdateCoinValue;
    }
    
    private void OnDisable()
    {
        ResetAnimation();
    }
    
    public void CloseButtonClick()
    {
        CloseImmediately();
    }
    
    public void ClaimButtonClick()
    {
        _dailyReward.InvokeGainReward();
        claimButton.interactable = false;
        ChangeUIWhenClaim();
       _dailyReward.PassToNextDay();
    }

    private void ChangeUIWhenClaim()
    {
        int dayNum = (int)dayState;
        dailyImage[dayNum].sprite = claimedFrameSprite;
        Transform pos = dailyImage[dayNum].transform;
        int coin = _dataController.currentGameData.coin;
        int coinAdded = coin + rewardsPrice[dayNum];
        coinRewardVip.PlayAnimation(coin, coinAdded, pos);
    }

    private void UpdateCoinValue(int value)
    {
        _dataController.currentGameData.coin = value;
        _uiManager.GetUI<UIMainMenu>().UpdateCoinText();
    }

    private void LoadRewardUI()
    {
        dayState = _dailyReward.currentDayState;
        int dayNum = (int)dayState;
        for (int i = 0; i < dayNum; i++)
        {
            dailyImage[i].sprite = claimedFrameSprite;
        }
        dailyImage[dayNum].sprite = _dailyReward.canClaim ? readyFrameSprite : waitFrameSprite;
        for (int i = dayNum + 1; i < dailyImage.Count; i++)
        {
            dailyImage[i].sprite = waitFrameSprite;
        }
    }

    private void DailyRewardAnimation()
    {
        dailyRewardPopup.DOAnchorPos(Vector2.zero, 0.6f).SetEase(Ease.OutSine);
    }

    private void ResetAnimation()
    {
        dailyRewardPopup.DOAnchorPos(new Vector2(0f, -1554f), 0f);
    }
}
