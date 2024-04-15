using UnityEngine;
using DG.Tweening;
using Zenject;

public class HandTut : MonoBehaviour
{
    [SerializeField] private Transform[] listPoint;
    [SerializeField] private GameObject tut;
    
    private DataController _dataController;
    private LinesDrawer _linesDrawer;

    [Inject]
    private void Construct(DataController dataController, LinesDrawer linesDrawer)
    {
        _dataController = dataController;
        _linesDrawer = linesDrawer;
    }
    private void OnEnable()
    {
        _linesDrawer.OnEndDraw += HideTut;
        SetAnimTut();
    }
    private void OnDisable()
    {
       _linesDrawer.OnEndDraw -= HideTut;
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
        
        tut.SetActive(false);
    }
}
