using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class HeadshotEffect : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform text, img;

    private AudioManager _audioManager;
    private ObjectPool _objectPool;
    private UIManager _uiManager;

    [Inject]
    private void Construct(AudioManager audioManager, ObjectPool objectPool, UIManager uiManager)
    {
        _audioManager = audioManager;
        _objectPool = objectPool;
        _uiManager = uiManager;
    }
    
    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void OnEnable()
    {
        ResetEffect();

        text.DOScale(1f, 0.4f).SetEase(Ease.InOutElastic).SetLoops(1);
        img.DOScale(0.23f, 0.2f).SetEase(Ease.InOutSine).SetLoops(1);
        if (_audioManager != null)
        {
            _audioManager.Play(Constant.AUDIO_SFX_HEADSHOT);
        }
        if (_uiManager.IsOpened<UIGameplay>())
        {
            _uiManager.GetUI<UIGameplay>().HandleCoinGainInGameplay(6);
        }
        FadeEffect();
    }

    private void FadeEffect()
    {
        meshRenderer.material.DOFade(0f, 1f).SetDelay(1.5f).SetEase(Ease.InOutSine);
        spriteRenderer.DOFade(0f, 1f).SetDelay(1.5f).SetEase(Ease.InOutSine).OnComplete(HideEffect);
    }
    private void HideEffect()
    {
        _objectPool.ReturnToPool(Constant.HEADSHOT_VFX, this.gameObject);
    }
    private void ResetEffect()
    {
        meshRenderer.material.DOFade(1f, 0f);
        spriteRenderer.DOFade(0.4f, 0f);
        text.DOScale(0.1f, 0f);
        img.DOScale(0.023f, 0f);
    }
}
