using UnityEngine;
using DG.Tweening;
using Zenject;

public class HandTut : MonoBehaviour
{
    [SerializeField] private Transform[] listPoint;
    [SerializeField] private GameObject tut;
    
    private DataController _dataController;

    [Inject]
    private void Construct(DataController dataController)
    {
        _dataController = dataController;
    }
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

        if (_dataController.currentGameData.levelDoneInGame.Count == 0)
        {
            for (int i = 0; i < 999; i++)
            {
                _dataController.currentGameData.levelDoneInGame.Add(0);
            }
        }

        //if (DataController.Instance.currentGameData.levelDoneInGame[0] == 0)
        //{
        //    tutAttack.SetActive(true);
        //}
        
        tut.SetActive(false);
    }
}
