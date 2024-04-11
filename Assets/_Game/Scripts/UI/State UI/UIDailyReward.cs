using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIDailyReward : UICanvas
{
    [SerializeField] private Sprite ready, wait, claimed;
    [SerializeField] List<Image> boxDay;
    [SerializeField] int[] rewards;
    [SerializeField] Button claimButton;
    [SerializeField] RectTransform popup;
    [SerializeField] CoinRewardPro coinReward;

    private DayState dayState;
    private void OnEnable()
    {
        LoadUIReward();
        AnimDailyRewardUI();

        // Check can press button ?
        if (DailyReward.Instance.canClaim) claimButton.interactable = true;
        else claimButton.interactable = false;

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
        DailyReward.Instance.InvokeGainReward();
        claimButton.interactable = false;
        ChangeUIWhenClaim();
        DailyReward.Instance.PassToNextDay();
    }

    public void ChangeUIWhenClaim()
    {
        int dayNum = (int)dayState;
        boxDay[dayNum].sprite = claimed;
        Transform pos = boxDay[dayNum].transform;
        int coin = DataController.Instance.currentGameData.coin;
        int coinAdded = coin + rewards[dayNum];
        coinReward.Anim(coin, coinAdded, pos);
    }

    public void UpdateCoin(int value)
    {
        DataController.Instance.currentGameData.coin = value;
        UIManager.Instance.GetUI<UIMainMenu>().UpdateCoinText();
    }
    public void LoadUIReward()
    {
        dayState = DailyReward.Instance.currentDayState;
        int dayNum = (int)dayState;
        for (int i = 0; i < dayNum; i++)
        {
            boxDay[i].sprite = claimed;
        }
        if (DailyReward.Instance.canClaim) boxDay[dayNum].sprite = ready;
        else boxDay[dayNum].sprite = wait;
        for (int i = dayNum + 1; i < boxDay.Count; i++)
        {
            boxDay[i].sprite = wait;
        }
    }
    public void AnimDailyRewardUI()
    {
        popup.DOAnchorPos(Vector2.zero, 0.6f).SetEase(Ease.OutSine);
    }
    public void ResetAnimDailyRewardUI()
    {
        popup.DOAnchorPos(new Vector2(0f, -1554f), 0f);
    }
}
