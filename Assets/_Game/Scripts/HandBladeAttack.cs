using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class HandBladeAttack : MonoBehaviour
{
    [SerializeField] GameObject hand, dashline, parent;
    private bool slided;
    
    private TimeManager _timeManager;

    [Inject]
    private void Construct(TimeManager timeManager)
    {
        _timeManager = timeManager;
    }
    
    private void OnEnable()
    {
        slided = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            slided = true;
        }
        if (slided)
        {
            hand.SetActive(false);
            dashline.SetActive(false);
            StartCoroutine(BackToGameplay());
            slided = false;
        }
    }
    public IEnumerator BackToGameplay()
    {
        yield return new WaitForSeconds(0.4f);
        _timeManager.BackNormalTime();
        parent.SetActive(false);
    }
}
