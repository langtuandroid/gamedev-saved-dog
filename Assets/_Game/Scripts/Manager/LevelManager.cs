using System;
using Integration;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class LevelManager : MonoBehaviour
{
    public event Action OnWinLevel; 
    public event Action OnLoseLevel;
    
    private const string INTEGRATIONS_COUNTER = "IntegrationsCounter";
    
    [SerializeField] private Level[] levels;
    
    private Level currentLevel;
    private int currentLevelIndex;
    private int currentSkinIndex;
    private int currentHp;
    private int stateIndex;
    private int loadLevelCount = 0;
    
    public Level CurrentLevel => currentLevel;
    public int CurrentLevelIndex => currentLevelIndex;
    public int StateIndex
    {
        get => stateIndex;
        set => stateIndex = value;
    }

    private DiContainer _diContainer;
    private DataController _dataController;
    private UIManager _uiManager;
    private SkinController _skinController;
    private LinesDrawer _linesDrawer;
    private AdMobController _adMobController;
    private IAPService _iapService;

   [Inject]
   private void Construct(DiContainer diContainer, DataController dataController, UIManager uiManager, SkinController skinController, LinesDrawer linesDrawer,AdMobController adMobController, IAPService iapService)
   {
       _diContainer = diContainer;
       _dataController = dataController;
       _uiManager = uiManager;
       _skinController = skinController;
       _linesDrawer = linesDrawer;
       _adMobController = adMobController;
       _iapService = iapService;
   }

   public void DespawnLevel()
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
        currentLevel = null;
    }

    public void LoadLevel(int level)
    {
        ShowIntegration();
        
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
        currentLevel.SetLevelNumberInGame(level);
        _linesDrawer.tilemap = currentLevel.GetComponentInChildren<Tilemap>();
        _uiManager.GetUI<UIGameplay>().UpdateLevelText(level + 1);
        _linesDrawer.OnLoadNewLevelOrUI();
        currentLevel.SetTime();
    }

    private void ShowIntegration()
    {
        loadLevelCount = PlayerPrefs.GetInt(INTEGRATIONS_COUNTER, 0);
        loadLevelCount++;
        
        if (loadLevelCount % 2 == 0)
        {
            _adMobController.ShowInterstitialAd();
        } else if (loadLevelCount % 3 == 0)
        {
            _iapService.ShowSubscriptionPanel();
        }
        if (loadLevelCount >= 3)
        {
            loadLevelCount = 0;
        }
        PlayerPrefs.SetInt(INTEGRATIONS_COUNTER, loadLevelCount);
        PlayerPrefs.Save();
    }
    
    private void LoadSkinForCharacter()
    {
        _skinController.LoadDataSkin();
        currentSkinIndex = _skinController.CurrentSkinIndex;
        currentHp = _skinController.CurrentHp;

        currentLevel.SetDogeSkin(currentSkinIndex, currentHp);
    }

    public void RetryLevel()
    {
        LoadLevel(currentLevelIndex);
    }
    
    public void CompleteLevel()
    {
        CheckLevelComplete();
        OnWinLevel?.Invoke();
        
        if (_dataController.currentGameData.levelDoneInGame[_dataController.currentGameData.currentLevelInProgress] == 0)
        {
            _dataController.currentGameData.levelDoneInGame[_dataController.currentGameData.currentLevelInProgress] = 1;
        }
    }
    
    public void LoseLevel()
    {
        OnLoseLevel?.Invoke();
    }
    
    private void CheckLevelComplete()
    {
        if (_dataController.currentGameData.levelDoneInGame.Count != 0)
        {
            return;
        }

        for (int i = 0; i < 999; i++)
        {
            _dataController.currentGameData.levelDoneInGame.Add(0);
        }
    }
    
    public void LoadNextLevel()
    {
        if (currentLevelIndex == _dataController.currentGameData.currentLevelInProgress)
        {
            _dataController.currentGameData.currentLevelInProgress++;
            LoadLevel(_dataController.currentGameData.currentLevelInProgress);
        } else if (currentLevelIndex < _dataController.currentGameData.currentLevelInProgress)
        {
            currentLevelIndex++;
            LoadLevel(currentLevelIndex);
        } else
        {
            _dataController.currentGameData.currentLevelInProgress = currentLevelIndex;
            _dataController.currentGameData.currentLevelInProgress++;
            LoadLevel(_dataController.currentGameData.currentLevelInProgress);
        }
    }
}
