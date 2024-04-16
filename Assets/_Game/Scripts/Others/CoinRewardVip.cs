using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Zenject;

public class CoinRewardVip : MonoBehaviour
{
    public event Action<int> OnUpdateCoin;
    
    [SerializeField] private GameObject pileCoin;
    
    private List<Vector3> initialPos = new List<Vector3>();
    private List<Quaternion> initialRotation = new List<Quaternion>();

    private int startValue, endValue;
    private float heightValue, widthValue, scaleValue;

    private AudioManager _audioManager;

    [Inject]
    private void Construct(AudioManager audioManager)
    {
        _audioManager = audioManager;
    }
    
    private void Awake()
    {
        for (int i = 0; i < pileCoin.transform.childCount; i++)
        {
            initialPos.Add(pileCoin.transform.GetChild(i).localPosition);
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
            pileCoin.transform.GetChild(i).localPosition = initialPos[i];
            pileCoin.transform.GetChild(i).rotation = initialRotation[i];
        }
    }
    
    public void PlayAnimation(int startValue, int endValue, Transform pilePos)
    {
        pileCoin.transform.position = pilePos.position;
        Vector2 offset = pileCoin.GetComponent<RectTransform>().anchoredPosition - Vector2.zero;
        SetDefault();

        var delay = 0f;

        pileCoin.gameObject.SetActive(true);

        for (int i = 0; i < pileCoin.transform.childCount; i++)
        {
            pileCoin.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
            pileCoin.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(new Vector2(456f, 870f * scaleValue) - offset, 0.8f).SetDelay(delay + 0.5f).SetEase(Ease.InBack).SetRelative(false);
            pileCoin.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);
            pileCoin.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(1.8f).SetEase(Ease.OutBack);

            delay += 0.1f;
        }

        _audioManager.Play(Constant.AUDIO_SFX_COIN);

        HandleUICoinText(startValue, endValue);
    }

    private void HandleUICoinText(int startValue, int endValue)
    {
        DOTween.Sequence().AppendInterval(1.25f).Append(DOTween.To(() => startValue, x => startValue = x, endValue, 1f).OnUpdate(() => OnUpdateCoin?.Invoke(startValue)));
    }
}
