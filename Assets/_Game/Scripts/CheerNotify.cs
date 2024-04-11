using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CheerNotify : Singleton<CheerNotify>
{
    public List<Transform> transformList;
    public List<SpriteRenderer> srList;

    public int streakKill;
    public float killInterval;
    public float counter;

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
        }
        else
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

    public void CheckStreak()
    {
        if (streakKill >=3 && streakKill < 6)
        {
            SetAnim(0);
            AudioManager.instance.Play(Constant.AUDIO_SFX_GOOD);
        }
        else if (streakKill >= 6 && streakKill < 10)
        {
            SetAnim(1);
            AudioManager.instance.Play(Constant.AUDIO_SFX_GREAT);
        }
        else if (streakKill >= 10 && streakKill < 12)
        {
            SetAnim(2);
            AudioManager.instance.Play(Constant.AUDIO_SFX_EXCELLENT);
        }
        else if (streakKill >= 12 && streakKill < 14)
        {
            SetAnim(3);
            AudioManager.instance.Play(Constant.AUDIO_SFX_AMAZING);
        }
        else if (streakKill >= 14 && streakKill < 16)
        {
            SetAnim(4);
            AudioManager.instance.Play(Constant.AUDIO_SFX_AWESOME);
        }
        else if (streakKill >= 16 && streakKill < 18)
        {
            SetAnim(5);
            AudioManager.instance.Play(Constant.AUDIO_SFX_INCREDIBLE);
        }
        else if (streakKill >= 18)
        {
            SetAnim(6);
            AudioManager.instance.Play(Constant.AUDIO_SFX_UNBELIEVABLE);
        }
    }
    public void CheckKill()
    {

        

        switch(streakKill)
        {
            case 3:
                SetAnim(0);
                AudioManager.instance.Play(Constant.AUDIO_SFX_GOOD);
                break;
            case 4:
                SetAnim(0);
                AudioManager.instance.Play(Constant.AUDIO_SFX_GOOD);
                break;
            case 5:
                SetAnim(0);
                AudioManager.instance.Play(Constant.AUDIO_SFX_GOOD);
                break;
            case 6:
                SetAnim(1);
                AudioManager.instance.Play(Constant.AUDIO_SFX_GREAT);
                break;
            case 7:
                SetAnim(1);
                AudioManager.instance.Play(Constant.AUDIO_SFX_GREAT);
                break;
            case 8:
                SetAnim(1);
                AudioManager.instance.Play(Constant.AUDIO_SFX_GREAT);
                break;
            case 9:
                SetAnim(1);
                AudioManager.instance.Play(Constant.AUDIO_SFX_GREAT);
                break;
            case 10:
                SetAnim(2);
                AudioManager.instance.Play(Constant.AUDIO_SFX_EXCELLENT);
                break;
            case 11:
                SetAnim(2);
                AudioManager.instance.Play(Constant.AUDIO_SFX_EXCELLENT);
                break;
            case 12:
                SetAnim(3);
                AudioManager.instance.Play(Constant.AUDIO_SFX_AMAZING);
                break;
            case 13:
                SetAnim(3);
                AudioManager.instance.Play(Constant.AUDIO_SFX_AMAZING);
                break;
            case 14:
                SetAnim(4);
                AudioManager.instance.Play(Constant.AUDIO_SFX_AWESOME);
                break;
            case 15:
                SetAnim(4);
                AudioManager.instance.Play(Constant.AUDIO_SFX_AWESOME);
                break;
            case 16:
                SetAnim(5);
                AudioManager.instance.Play(Constant.AUDIO_SFX_INCREDIBLE);
                break;
            case 17:
                SetAnim(5);
                AudioManager.instance.Play(Constant.AUDIO_SFX_INCREDIBLE);
                break;
            case 18:
                SetAnim(6);
                AudioManager.instance.Play(Constant.AUDIO_SFX_UNBELIEVABLE);
                break;
            case 19:
                SetAnim(6);
                AudioManager.instance.Play(Constant.AUDIO_SFX_UNBELIEVABLE);
                break;
            case 20:
                SetAnim(6);
                AudioManager.instance.Play(Constant.AUDIO_SFX_UNBELIEVABLE);
                break;
            case 21:
                SetAnim(6);
                AudioManager.instance.Play(Constant.AUDIO_SFX_UNBELIEVABLE);
                break;
            case 22:
                SetAnim(6);
                AudioManager.instance.Play(Constant.AUDIO_SFX_UNBELIEVABLE);
                break;
            default:
                break;
        }
    }

    public void SetAnim(int index)
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
