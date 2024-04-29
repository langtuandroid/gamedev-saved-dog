using Integration;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] 
    private AdMobController _adMobController;
    [SerializeField] 
    private IAPService _iapService;
    [SerializeField]
    private DataController _dataController;
    [SerializeField]
    private DataPersistence _dataPersistence;
    public override void InstallBindings()
    {
        Container.Bind<DataPersistence>().FromInstance(_dataPersistence).AsSingle().NonLazy();
        Container.Bind<DataController>().FromInstance(_dataController).AsSingle().NonLazy();
        Container.Bind<IAPService>().FromInstance(_iapService).AsSingle().NonLazy();
        Container.Bind<AdMobController>().FromInstance(_adMobController).AsSingle().NonLazy();
        
        Container.Bind<BannerViewController>().AsSingle().NonLazy();
        Container.Bind<InterstitialAdController>().AsSingle().NonLazy();
        Container.Bind<RewardedAdController>().AsSingle().NonLazy();
    }
}
