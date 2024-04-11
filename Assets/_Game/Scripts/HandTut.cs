using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandTut : MonoBehaviour
{
    [SerializeField] private Transform[] listPoint;
    [SerializeField] private GameObject tut;
    private void OnEnable()
    {
        LinesDrawer.instance.OnEndDraw += HideTut;
        SetAnimTut();
    }
    private void OnDisable()
    {
        LinesDrawer.instance.OnEndDraw -= HideTut;
    }
    private void SetAnimTut()
    {
        Vector3[] listPoints = new Vector3[listPoint.Length];

        for (int i = 0; i < listPoint.Length; i++)
        {
            listPoints[i] = listPoint[i].position;
        }

        transform.DOPath(listPoints, 3f, PathType.Linear).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }
    private void HideTut()
    {

        if (DataController.Instance.currentGameData.levelDoneInGame.Count == 0)
        {
            for (int i = 0; i < 999; i++)
            {
                DataController.Instance.currentGameData.levelDoneInGame.Add(0);
            }
        }

        //if (DataController.Instance.currentGameData.levelDoneInGame[0] == 0)
        //{
        //    tutAttack.SetActive(true);
        //}
        
        tut.SetActive(false);
    }
}
