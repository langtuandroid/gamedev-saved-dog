using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandBladeAttack : MonoBehaviour
{
    [SerializeField] GameObject hand, dashline, parent;
    private bool slided;
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
        TimeManager.Instance.BackNormalTime();
        parent.SetActive(false);
    }
}
