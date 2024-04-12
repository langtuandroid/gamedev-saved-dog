using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

public class UIDailyReward : UICanvas
{
    [SerializeField] private Sprite ready, wait, claimed;
    [SerializeField] List<Image> boxDay;
    [SerializeField] int[] rewards;
    [SerializeField] Button claimButton;
    [SerializeField] RectTransform popup;
    [SerializeField] CoinRewardPro coinReward;

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
        LoadUIReward();
        AnimDailyRewardUI();
        
        claimButton.interactable = _dailyReward.canClaim;

        coinReward.updateCoin += UpdateCoin;
    }
    
    private void OnDisable()
    {
        ResetAnimDailyRewardUI();
    }
    
    public void CloseButton()
    {
        CloseDirectly();
    }
    
    public void ClaimButton()
    {
        _dailyReward.InvokeGainReward();
        claimButton.interactable = false;
        ChangeUIWhenClaim();
       _dailyReward.PassToNextDay();
    }

    private void ChangeUIWhenClaim()
    {
        int dayNum = (int)dayState;
        boxDay[dayNum].sprite = claimed;
        Transform pos = boxDay[dayNum].transform;
        int coin = _dataController.currentGameData.coin;
        int coinAdded = coin + rewards[dayNum];
        coinReward.Anim(coin, coinAdded, pos);
    }

    private void UpdateCoin(int value)
    {
        _dataController.currentGameData.coin = value;
        UIManager.Instance.GetUI<UIMainMenu>().UpdateCoinText();
    }

    private void LoadUIReward()
    {
        dayState = _dailyReward.currentDayState;
        int dayNum = (int)dayState;
        for (int i = 0; i < dayNum; i++)
        {
            boxDay[i].sprite = claimed;
        }
        boxDay[dayNum].sprite = _dailyReward.canClaim ? ready : wait;
        for (int i = dayNum + 1; i < boxDay.Count; i++)
        {
            boxDay[i].sprite = wait;
        }
    }

    private void AnimDailyRewardUI()
    {
        popup.DOAnchorPos(Vector2.zero, 0.6f).SetEase(Ease.OutSine);
    }

    private void ResetAnimDailyRewardUI()
    {
        popup.DOAnchorPos(new Vector2(0f, -1554f), 0f);
    }
}
