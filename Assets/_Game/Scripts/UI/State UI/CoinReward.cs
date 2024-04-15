using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class CoinReward : MonoBehaviour
{
    [SerializeField] private GameObject pileCoin;
    [SerializeField] private UIWin uiWin;
    
    private List<Vector3> initialPosition = new List<Vector3>();
    private List<Quaternion> initialRotation = new List<Quaternion>();
    private int startValue, endValue;
    private float heightValue, widthValue, scaleValue;
    
    private DataController _dataController;
    private AudioManager _audioManager;

    [Inject]
    private void Construct(DataController dataController, AudioManager audioManager)
    {
        _dataController = dataController;
        _audioManager = audioManager;
    }

    private void Awake()
    {
        for(int i = 0; i < pileCoin.transform.childCount; i++)
        {
            initialPosition.Add(pileCoin.transform.GetChild(i).position);
            initialRotation.Add(pileCoin.transform.GetChild(i).rotation);
        }
        heightValue = Screen.height;
        widthValue = Screen.width;
        scaleValue = 0.5625f / (widthValue / heightValue);
    }

    public void Init()
    {
        for (int i = 0; i < pileCoin.transform.childCount; i++)
        {
            initialPosition.Add(pileCoin.transform.GetChild(i).position);
            initialRotation.Add(pileCoin.transform.GetChild(i).rotation);
        }
        heightValue = Screen.height;
        widthValue = Screen.width;
        scaleValue = 0.5625f / (widthValue / heightValue);
    }

    private void SetDefault()
    {
        for (int i = 0; i < pileCoin.transform.childCount; i++)
        {
            pileCoin.transform.GetChild(i).position = initialPosition[i];
            pileCoin.transform.GetChild(i).rotation = initialRotation[i];
        }
    }
    
    public void AnimCoinFly(int addCoin, bool canNextLevel)
    {
        if (addCoin == 0)
        {
            return;
        }

        SetDefault();

        var delay = 0f;

        pileCoin.gameObject.SetActive(true);

        for(int i = 0; i < pileCoin.transform.childCount; i++)
        {
            pileCoin.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
            pileCoin.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(new Vector2(456f, 870f*scaleValue), 0.8f).SetDelay(delay + 0.5f).SetEase(Ease.InBack);
            pileCoin.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);
            pileCoin.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(1.8f).SetEase(Ease.OutBack);

            delay += 0.1f;
        }

        _audioManager.Play(Constant.AUDIO_SFX_COIN);
        startValue = _dataController.currentGameData.coin;
        endValue = _dataController.currentGameData.coin + addCoin;
        DOTween.Sequence().AppendInterval(1.25f).Append(DOTween.To(() => startValue, x => startValue = x, endValue, 1f).OnUpdate(() => UpdateValue(startValue)).OnComplete(() => 
        { 
            if (canNextLevel)
            {
                uiWin.StartCoroutine(uiWin.HandleNextLevel());
            }
        }));

    }

    private void UpdateValue(int value)
    {
        uiWin.UpdateCoinText(value);
        _dataController.currentGameData.coin = value;
    }
}
