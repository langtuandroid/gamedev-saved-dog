using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BladeTut : MonoBehaviour
{
    [SerializeField] GameObject handAttack;
    
    private TimeManager _timeManager;

    [Inject]
    private void Construct(TimeManager timeManager)
    {
        _timeManager = timeManager;
    }
    
    private void OnEnable()
    {
        Invoke("AnimTut", 1.5f);
    }
    private void AnimTut()
    {
        //UIManager.Instance.GetUI<UIGameplay>().ShowTut();
        _timeManager.SlowTime();
        handAttack.SetActive(true);
    }
}
