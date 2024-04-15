using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class CoinReward : MonoBehaviour
{
    [SerializeField] private GameObject pileCoin;
    [SerializeField] private UIWin uiWin;
    public List<Vector3> initialPos;
    public List<Quaternion> initialRotation;

    public int startValue, endValue;
    private float height, width, scale;
    
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
            initialPos.Add(pileCoin.transform.GetChild(i).position);
            initialRotation.Add(pileCoin.transform.GetChild(i).rotation);
        }
        height = Screen.height;
        width = Screen.width;
        scale = 0.5625f / (width / height);
    }

    public void OnInit()
    {
        for (int i = 0; i < pileCoin.transform.childCount; i++)
        {
            initialPos.Add(pileCoin.transform.GetChild(i).position);
            initialRotation.Add(pileCoin.transform.GetChild(i).rotation);
        }
        height = Screen.height;
        width = Screen.width;
        scale = 0.5625f / (width / height);
    }

    public void SetDefault()
    {
        for (int i = 0; i < pileCoin.transform.childCount; i++)
        {
            pileCoin.transform.GetChild(i).position = initialPos[i];
            pileCoin.transform.GetChild(i).rotation = initialRotation[i];
        }
    }
    public void AnimCoinFly(int addCoin, bool canNextLevel)
    {
        if (addCoin == 0)
        {
            //uiWin.HandleNextLevel();
            return;
        }

        SetDefault();

        var delay = 0f;

        pileCoin.gameObject.SetActive(true);

        for(int i = 0; i < pileCoin.transform.childCount; i++)
        {
            pileCoin.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
            pileCoin.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(new Vector2(456f, 870f*scale), 0.8f).SetDelay(delay + 0.5f).SetEase(Ease.InBack);
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
