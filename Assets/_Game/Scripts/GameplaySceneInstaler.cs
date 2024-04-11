using UnityEngine;
using Zenject;

public class GameplaySceneInstaler : MonoInstaller
{
  [SerializeField]
  private GameManager _gameManager;
  [SerializeField]
  private LevelManager _levelManager;
  [SerializeField]
  private DataPersistence _dataPersistence;
  public override void InstallBindings()
  {
    Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle().NonLazy();
    Container.Bind<LevelManager>().FromInstance(_levelManager).AsSingle().NonLazy();
    Container.Bind<DataPersistence>().FromInstance(_dataPersistence).AsSingle().NonLazy();
  }
}
