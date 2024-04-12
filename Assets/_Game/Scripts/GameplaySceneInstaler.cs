using UnityEngine;
using Zenject;

public class GameplaySceneInstaler : MonoInstaller
{
  [Header("Managers"), Space]
  [SerializeField]
  private GameManager _gameManager;
  [SerializeField]
  private LevelManager _levelManager;
  [SerializeField]
  private DataPersistence _dataPersistence;
  [SerializeField]
  private DataController _dataController;
  [SerializeField]
  private TimeManager _timeManager;
  [SerializeField]
  private AudioManager _audioManager;
  [SerializeField]
  private UIManager _uiManager;
  [SerializeField]
  private SkinController _skinController;


  [Header("Other"), Space]
  [SerializeField]
  private ObjectPool _objectPool;
  [SerializeField]
  private DailyReward _dailyReward;
  public override void InstallBindings()
  {
    Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle().NonLazy();
    Container.Bind<LevelManager>().FromInstance(_levelManager).AsSingle().NonLazy();
    Container.Bind<DataPersistence>().FromInstance(_dataPersistence).AsSingle().NonLazy();
    Container.Bind<DataController>().FromInstance(_dataController).AsSingle().NonLazy();
    Container.Bind<TimeManager>().FromInstance(_timeManager).AsSingle().NonLazy();
    Container.Bind<AudioManager>().FromInstance(_audioManager).AsSingle().NonLazy();
    Container.Bind<UIManager>().FromInstance(_uiManager).AsSingle().NonLazy();
    Container.Bind<SkinController>().FromInstance(_skinController).AsSingle().NonLazy();
    
    Container.Bind<ObjectPool>().FromInstance(_objectPool).AsSingle().NonLazy();
    Container.Bind<DailyReward>().FromInstance(_dailyReward).AsSingle().NonLazy();
  }
}
