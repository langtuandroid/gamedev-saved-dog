using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeTut : MonoBehaviour
{
    [SerializeField] GameObject handAttack;
    private void OnEnable()
    {
        Invoke("AnimTut", 1.5f);
    }
    private void AnimTut()
    {
        //UIManager.Instance.GetUI<UIGameplay>().ShowTut();
        TimeManager.Instance.SlowTime();
        handAttack.SetActive(true);
    }
}
