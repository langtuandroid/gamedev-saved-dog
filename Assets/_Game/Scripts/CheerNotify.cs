using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class CheerNotify : MonoBehaviour
{
    public List<Transform> transformList;
    public List<SpriteRenderer> srList;

    public int streakKill;
    public float killInterval;
    public float counter;

    private AudioManager _audioManager;

    [Inject]
    private void Construct(AudioManager audioManager)
    {
        _audioManager = audioManager;
    }
    
    private void Start()
    {
        streakKill = 0;
        counter = 0;
    }

    private void Update()
    {
        if (counter > 0)
        {
            counter -= Time.deltaTime;
        } else
        {
            CheckStreak();
            streakKill = 0;
        }
    }

    public void KeepStreak()
    {
        streakKill++;
        counter = killInterval;
    }

    private void CheckStreak()
    {
        if (streakKill is >= 3 and < 6)
        {
            SetAnim(0);
            _audioManager.Play(Constant.AUDIO_SFX_GOOD);
        } else if (streakKill is >= 6 and < 10)
        {
            SetAnim(1);
            _audioManager.Play(Constant.AUDIO_SFX_GREAT);
        } else if (streakKill is >= 10 and < 12)
        {
            SetAnim(2);
            _audioManager.Play(Constant.AUDIO_SFX_EXCELLENT);
        } else if (streakKill is >= 12 and < 14)
        {
            SetAnim(3);
            _audioManager.Play(Constant.AUDIO_SFX_AMAZING);
        } else if (streakKill is >= 14 and < 16)
        {
            SetAnim(4);
            _audioManager.Play(Constant.AUDIO_SFX_AWESOME);
        } else if (streakKill is >= 16 and < 18)
        {
            SetAnim(5);
            _audioManager.Play(Constant.AUDIO_SFX_INCREDIBLE);
        } else if (streakKill >= 18)
        {
            SetAnim(6);
            _audioManager.Play(Constant.AUDIO_SFX_UNBELIEVABLE);
        }
    }

    private void SetAnim(int index)
    {
        transformList[index].gameObject.SetActive(true);
        transformList[index].DOMove(new Vector3(0f, -0.6f, 0f), 0f);
        srList[index].DOFade(1f, 0f);


        transformList[index].DOMove(Vector3.zero, 0.9f).SetEase(Ease.OutQuad);
        srList[index].DOFade(0f, 0.9f).SetEase(Ease.OutQuad).OnComplete(() => Hide(index));

    }
    
    private void Hide(int index)
    {
        transformList[index].gameObject.SetActive(false);
    }
}
