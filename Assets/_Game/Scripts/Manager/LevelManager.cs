using System;
using System.Collections;
using System.Collections.Generic;
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
    
   [Inject] private DiContainer _diContainer;
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
        if (DataController.Instance.currentGameData.levelDoneInGame[DataController.Instance.currentGameData.currentLevelInProgress] == 0)
        {
            DataController.Instance.currentGameData.levelDoneInGame[DataController.Instance.currentGameData.currentLevelInProgress] = 1;
            //Debug.Log(DataController.Instance.currentGameData.currentLevelInProgress + "--");
        }
    }
    public void OnLose()
    {
        OnLoseLevel?.Invoke();
    }
    private void CheckLevelDone()
    {
        if (DataController.Instance.currentGameData.levelDoneInGame.Count == 0)
        {
            for (int i = 0; i < 999; i++)
            {
                DataController.Instance.currentGameData.levelDoneInGame.Add(0);
            }
        }
    }
    public void OnLoadNextLevel()
    {
        if (currentLevelIndex == DataController.Instance.currentGameData.currentLevelInProgress)
        {
            DataController.Instance.currentGameData.currentLevelInProgress++;
            OnLoadLevel(DataController.Instance.currentGameData.currentLevelInProgress);
        }
        else if (currentLevelIndex < DataController.Instance.currentGameData.currentLevelInProgress)
        {
            currentLevelIndex++;
            OnLoadLevel(currentLevelIndex);
        }
        else
        {
            DataController.Instance.currentGameData.currentLevelInProgress = currentLevelIndex;
            DataController.Instance.currentGameData.currentLevelInProgress++;
            OnLoadLevel(DataController.Instance.currentGameData.currentLevelInProgress);
        }
    }
}
