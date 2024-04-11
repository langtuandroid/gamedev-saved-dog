using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CoinRewardPro : MonoBehaviour
{
    [SerializeField] private GameObject pileCoin;
    public List<Vector3> initialPos;
    public List<Quaternion> initialRotation;
    public event Action<int> updateCoin;

    private int startValue, endValue;
    private float height, width, scale;

    private void Awake()
    {
        for (int i = 0; i < pileCoin.transform.childCount; i++)
        {
            initialPos.Add(pileCoin.transform.GetChild(i).localPosition);
            initialRotation.Add(pileCoin.transform.GetChild(i).rotation);
        }
        height = Screen.height;
        width = Screen.width;
        scale = 0.5625f / (width / height);
    }
    private void SetDefault()
    {
        for (int i = 0; i < pileCoin.transform.childCount; i++)
        {
            pileCoin.transform.GetChild(i).localPosition = initialPos[i];
            pileCoin.transform.GetChild(i).rotation = initialRotation[i];
        }
    }
    public void Anim(int startValue, int endValue, Transform pilePos)
    {
        pileCoin.transform.position = pilePos.position;
        Vector2 offset = (Vector2)pileCoin.GetComponent<RectTransform>().anchoredPosition - Vector2.zero;
        SetDefault();

        var delay = 0f;

        pileCoin.gameObject.SetActive(true);

        for (int i = 0; i < pileCoin.transform.childCount; i++)
        {
            pileCoin.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
            pileCoin.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(new Vector2(456f, 870f * scale) - offset, 0.8f).SetDelay(delay + 0.5f).SetEase(Ease.InBack).SetRelative(false);
            pileCoin.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);
            pileCoin.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(1.8f).SetEase(Ease.OutBack);

            delay += 0.1f;
        }

        AudioManager.instance.Play(Constant.AUDIO_SFX_COIN);

        HandleUICoinText(startValue, endValue);
    }

    private void HandleUICoinText(int startValue, int endValue)
    {
        DOTween.Sequence().AppendInterval(1.25f).Append(DOTween.To(() => startValue, x => startValue = x, endValue, 1f).OnUpdate(() => updateCoin(startValue)));
    }
    
}
