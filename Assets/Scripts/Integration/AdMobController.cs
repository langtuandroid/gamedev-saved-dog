using GoogleMobileAds.Api;
using UnityEngine;
using Zenject;

namespace Integration  
{
	/// <summary>
	/// https://github.com/googleads/googleads-mobile-unity/tree/main/samples/HelloWorld/Assets/Scripts
	/// https://github.com/googleads/googleads-mobile-unity/releases
	/// https://developers.google.com/admob/unity/interstitial#ios
	/// </summary>

	public class AdMobController : MonoBehaviour
	{
		private bool _noAds;
		public string noAdsKey = "NoAds";

		[SerializeField] private AdMobSettings _settings;
		[SerializeField] private bool IsProdaction;
		
		private BannerViewController _bannerViewController;
		private InterstitialAdController _interstitialAdController;
		private RewardedAdController _rewardedAdController;


		[Inject]
		private void Construct(
			BannerViewController bannerViewController,
			InterstitialAdController interstitialAdController,
			RewardedAdController rewardedAdController)
		{
			_bannerViewController = bannerViewController;
			_interstitialAdController = interstitialAdController;
			_rewardedAdController = rewardedAdController;
		}
		private void Awake()
		{
			MobileAds.Initialize(initStatus =>
			{
				Debug.Log("InitAds = " + initStatus);
			});
		}

		private void Start()
		{
			_bannerViewController.BannerId = IsProdaction ? _settings.BannerID : _settings.BannerTestID;
			_interstitialAdController.InterstitialId = IsProdaction ? _settings.InterstitialID : _settings.InterstitialTestID;
			_rewardedAdController.RewardedId = IsProdaction ? _settings.RewardedID : _settings.RewardedTestID;
			LoadAllAds();
		}

		private void LoadAllAds()
		{
			_noAds = PlayerPrefs.GetInt(noAdsKey, 0) == 1;
			Debug.Log("_noAds=" +_noAds);
			if (!_noAds)
			{
				RequestBanner();
				_interstitialAdController.LoadAd();
			}

			_rewardedAdController.LoadAd();
		}

		public void RemoveAds()
		{
			PlayerPrefs.SetInt(noAdsKey, 1);
			ShowBanner(false);
		}
		
// Banner	
		public void RequestBanner()
		{
			_bannerViewController.CreateBannerView();
			_bannerViewController.LoadAd();
			_bannerViewController.HideAd();
		}

		public void ShowBanner(bool show)
		{
			_noAds = PlayerPrefs.GetInt(noAdsKey, 0) == 1;
			if (!_noAds)
			{
				if (show)
				{
					_bannerViewController.ShowAd();
				}
				else
				{
					_bannerViewController.HideAd();
				}
			}
		}

// Interstitial		
		public void ShowInterstitialAd()
		{
			_noAds = PlayerPrefs.GetInt(noAdsKey, 0) == 1;
			if (!_noAds)
			{
				_interstitialAdController.ShowAd();
			}
		}

// Rewarded			
		public void ShowRewardedAd()
		{
			_rewardedAdController.ShowAd();
		}
	}
}


