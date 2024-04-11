using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level[] levels;
    public Level currentLevel;

    //[HideInInspector] public int[] levelDone = new int[100];
    //private int levelIndexInProgress; public int LevelIndexInProgress { get { return levelIndexInProgress; } }
    private int currentLevelIndex; public int CurrentLevelIndex { get { return currentLevelIndex; } }

    private int currentSkinIndex, currentHp;

    public event Action OnWinLevel, OnLoseLevel;
    public int stateIndex;
    
    private DiContainer _diContainer;
    private DataController _dataController;

   [Inject]
   private void Construct(DiContainer diContainer, DataController dataController)
   {
       _diContainer = diContainer;
       _dataController = dataController;
   }
    void Start()
    {
        //levelIndexInProgress = 0;
    }

    public void Despawn()
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
        currentLevel = null;
    }

    public void OnLoadLevel(int level)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
        if (level >= levels.Length)
        {
            level = 0;
        }
        currentLevel = _diContainer.InstantiatePrefabForComponent<Level>(levels[level]);

        LoadSkinForCharacter();

        currentLevelIndex = level;
        currentLevel.levelNumberInGame = level;
        LinesDrawer.instance.tilemap = currentLevel.GetComponentInChildren<Tilemap>();
        UIManager.Instance.GetUI<UIGameplay>().UpdateLevelText(level + 1);
        LinesDrawer.instance.OnLoadNewLevelOrUI();
        currentLevel.SetTime();
    }

    private void LoadSkinForCharacter()
    {
        SkinController.Instance.LoadDataSkin();
        currentSkinIndex = SkinController.Instance.currentSkinIndex;
        currentHp = SkinController.Instance.currentHp;

        currentLevel.SetSkin(currentSkinIndex, currentHp);
    }

    public void OnRetry()
    {
        OnLoadLevel(currentLevelIndex);
    }
    public void OnWin()
    {
        CheckLevelDone();
        OnWinLevel?.Invoke();
        //Debug.Log("leveldone?--" + DataController.Instance.currentGameData.levelDoneInGame[DataController.Instance.currentGameData.currentLevelInProgress]);
        if (_dataController.currentGameData.levelDoneInGame[_dataController.currentGameData.currentLevelInProgress] == 0)
        {
            _dataController.currentGameData.levelDoneInGame[_dataController.currentGameData.currentLevelInProgress] = 1;
            //Debug.Log(DataController.Instance.currentGameData.currentLevelInProgress + "--");
        }
    }
    public void OnLose()
    {
        OnLoseLevel?.Invoke();
    }
    private void CheckLevelDone()
    {
        if (_dataController.currentGameData.levelDoneInGame.Count == 0)
        {
            for (int i = 0; i < 999; i++)
            {
                _dataController.currentGameData.levelDoneInGame.Add(0);
            }
        }
    }
    public void OnLoadNextLevel()
    {
        if (currentLevelIndex == _dataController.currentGameData.currentLevelInProgress)
        {
            _dataController.currentGameData.currentLevelInProgress++;
            OnLoadLevel(_dataController.currentGameData.currentLevelInProgress);
        }
        else if (currentLevelIndex < _dataController.currentGameData.currentLevelInProgress)
        {
            currentLevelIndex++;
            OnLoadLevel(currentLevelIndex);
        }
        else
        {
            _dataController.currentGameData.currentLevelInProgress = currentLevelIndex;
            _dataController.currentGameData.currentLevelInProgress++;
            OnLoadLevel(_dataController.currentGameData.currentLevelInProgress);
        }
    }
}
