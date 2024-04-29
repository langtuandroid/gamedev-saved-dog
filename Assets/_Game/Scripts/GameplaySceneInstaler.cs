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
  [SerializeField]
  private CheerNotify _cheerNotify;
  [SerializeField]
  private PhoneVibrate _phoneVibrate;
  [SerializeField]
  private LinesDrawer _linesDrawer;
  
  public override void InstallBindings()
  {
    Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle().NonLazy();
    Container.Bind<LevelManager>().FromInstance(_levelManager).AsSingle().NonLazy();
    Container.Bind<TimeManager>().FromInstance(_timeManager).AsSingle().NonLazy();
    Container.Bind<AudioManager>().FromInstance(_audioManager).AsSingle().NonLazy();
    Container.Bind<UIManager>().FromInstance(_uiManager).AsSingle().NonLazy();
    Container.Bind<SkinController>().FromInstance(_skinController).AsSingle().NonLazy();
    
    Container.Bind<ObjectPool>().FromInstance(_objectPool).AsSingle().NonLazy();
    Container.Bind<DailyReward>().FromInstance(_dailyReward).AsSingle().NonLazy();
    Container.Bind<CheerNotify>().FromInstance(_cheerNotify).AsSingle().NonLazy();
    Container.Bind<PhoneVibrate>().FromInstance(_phoneVibrate).AsSingle().NonLazy();
    Container.Bind<LinesDrawer>().FromInstance(_linesDrawer).AsSingle().NonLazy();
  }
}
