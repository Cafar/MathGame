using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

#if GOOGLE_MOBILE_ADS
using GoogleMobileAds.Api;
#endif

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

#if CHARTBOOST
using ChartboostSDK;
#endif

#if UNITY_IOS
using ADBannerView = UnityEngine.iOS.ADBannerView;
using ADInterstitialAd = UnityEngine.iOS.ADInterstitialAd;
#endif



public class AdsManager : MonoBehaviour 
{
	public bool rewardedVideoAlwaysReadyInSimulator = true;
	public bool rewardedVideoAlwaysSuccessInSimulator = true;


	public int numberOfLoseToShowInterstitial = 3;

	#if UNITY_IOS
	private ADBannerView banner = null;
	private ADInterstitialAd fullscreenAd = null;
	#endif

	#if STAN_ASSET_GOOGLEMOBILEADS || STAN_ASSET_ANDROIDNATIVE || GOOGLE_MOBILE_ADS
	//replace with your ids
	public string AdmobBannerIdIOS = "ca-app-pub-4501064062171971/5799911242";
	public string AdmobInterstitialIdIOS = "ca-app-pub-4501064062171971/7276644447";
	public string AdmobBannerIdANDROID = "ca-app-pub-4501064062171971/9943794444";
	public string AdmobInterstitialIdANDROID = "ca-app-pub-4501064062171971/2420527649";
	#endif

	#if UNITY_IOS && (GOOGLE_MOBILE_ADS || STAN_ASSET_GOOGLEMOBILEADS || STAN_ASSET_ANDROIDNATIVE)
	public bool useAdmob;
	#endif

	#if GOOGLE_MOBILE_ADS
	BannerView bannerView;
	AdRequest requestBanner;
	InterstitialAd interstitial;
	AdRequest requestInterstitial;
	#endif


	#if STAN_ASSET_GOOGLEMOBILEADS
	private static Dictionary<string, GoogleMobileAdBanner> _registerdBanners = null;
	#endif

	#if STAN_ASSET_ANDROIDNATIVE
	private static Dictionary<string, GoogleMobileAdBanner> _registerdBanners = null;
	#endif


	void Awake()
	{
		Set ();
	}

	IEnumerator Start()
	{

		#if UNITY_IOS
		fullscreenAd = new ADInterstitialAd();
		ADInterstitialAd.onInterstitialWasLoaded  += OnFullscreenLoaded;
		#if !UNITY_4_6 && !UNITY_5_0 && !UNITY_5_1
		ADInterstitialAd.onInterstitialWasViewed  += OnFullscreenViewed;
		#endif
		#endif

		#if STAN_ASSET_GOOGLEMOBILEADS
		if(!GoogleMobileAd.IsInited) {
		GoogleMobileAd.Init();
		}
		#endif

		#if STAN_ASSET_ANDROIDNATIVE
		if(!AndroidAdMobController.Instance.IsInited) {
		Set();
		}
		#endif

		#if CHARTBOOST
		Chartboost.setAutoCacheAds(true);

		Chartboost.showInterstitial (CBLocation.Startup);

		Chartboost.cacheInterstitial (CBLocation.Default);

		Chartboost.cacheRewardedVideo(CBLocation.Default);
		#endif


		GC.Collect ();

		Resources.UnloadUnusedAssets ();

		Application.targetFrameRate = 60;


		yield return new WaitForSeconds (1);

		ShowBanner ();

//		popupContinueParent.gameObject.SetActive (true);
//
//		scalePopupContinueParent = 0f;
//
//		fillAmountPopupContinue = 1f;
//
	}

	#if GOOGLE_MOBILE_ADS
	void RequestInterstitial()
	{
		if (Application.isMobilePlatform) 
		{
			requestInterstitial = new AdRequest.Builder ().Build ();
			interstitial.LoadAd (requestInterstitial);
		}
	}
	#endif

	public void Set()
	{

		#if GOOGLE_MOBILE_ADS
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			bannerView = new BannerView(AdmobBannerIdIOS, AdSize.SmartBanner, AdPosition.Bottom);
		else if (Application.platform == RuntimePlatform.Android)
			bannerView = new BannerView(AdmobBannerIdANDROID, AdSize.SmartBanner, AdPosition.Bottom);

		requestBanner = new AdRequest.Builder().Build();

		if (Application.platform == RuntimePlatform.IPhonePlayer)
			interstitial = new InterstitialAd(AdmobInterstitialIdIOS);
		else if (Application.platform == RuntimePlatform.Android)
			interstitial = new InterstitialAd(AdmobInterstitialIdANDROID);


		RequestInterstitial();


		#endif


		#if STAN_ASSET_GOOGLEMOBILEADS
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
		GoogleMobileAdSettings.Instance.IOS_BannersUnitId = AdmobBannerIdIOS;
		GoogleMobileAdSettings.Instance.IOS_InterstisialsUnitId = AdmobInterstitialIdIOS;
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
		GoogleMobileAdSettings.Instance.IOS_BannersUnitId = AdmobBannerIdANDROID;
		GoogleMobileAdSettings.Instance.IOS_InterstisialsUnitId = AdmobInterstitialIdANDROID;
		}
		#endif

		#if STAN_ASSET_ANDROIDNATIVE

		if(!string.IsNullOrEmpty(AdmobBannerIdANDROID))
		AndroidAdMobController.Instance.SetBannersUnitID(AdmobBannerIdANDROID);

		if(!string.IsNullOrEmpty(AdmobInterstitialIdANDROID))
		AndroidAdMobController.Instance.SetInterstisialsUnitID(AdmobInterstitialIdANDROID);

		if(!string.IsNullOrEmpty(AdmobBannerIdANDROID) && !string.IsNullOrEmpty(AdmobInterstitialIdANDROID))
		AndroidAdMobController.Instance.Init (AdmobBannerIdANDROID,AdmobInterstitialIdANDROID);
		else if(!string.IsNullOrEmpty(AdmobBannerIdANDROID))
		AndroidAdMobController.Instance.Init (AdmobBannerIdANDROID);
		#endif
	}

	#if GOOGLE_MOBILE_ADS
	// Called when an ad request has successfully loaded.
	void HandleAdLoaded(object sender, EventArgs e)
	{

	}
	// Called when an ad request failed to load.
	void HandleAdFailedToLoad(object sender, EventArgs e)
	{
		Invoke ("ShowBanner", 10);
	}
	// Called when an ad is clicked.
	void HandleAdOpened(object sender, EventArgs e)
	{

	}
	// Called when the user is about to return to the app after an ad click.
	void HandleAdClosing(object sender, EventArgs e)
	{

	}
	// Called when the user returned from the app after an ad click.
	void HandleAdClosed(object sender, EventArgs e)
	{

	}
	// Called when the ad click caused the user to leave the application.
	void HandleAdLeftApplication(object sender, EventArgs e)
	{

	}

	public void Show_Banner()
	{
		if(bannerView!=null)
			bannerView.Show();
	}

	public void Hide_Banner()
	{
		if(bannerView!=null)
			bannerView.Hide();
	}


	#endif

	private void ShowBanner() 
	{
		#if UNITY_IOS
		banner = new ADBannerView(ADBannerView.Type.Banner, ADBannerView.Layout.BottomCenter);
		ADBannerView.onBannerWasClicked += OnBannerClicked;
		ADBannerView.onBannerWasLoaded += OnBannerLoaded;
		#if !UNITY_4_6 && !UNITY_5_0 && !UNITY_5_1
		ADBannerView.onBannerFailedToLoad += OnBannerFailedToLoad;
		#endif
		#endif

		#if GOOGLE_MOBILE_ADS
	
		if(Application.isMobilePlatform)
		{
		bannerView.LoadAd(requestBanner);



		bannerView.AdLoaded -= HandleAdLoaded;
		bannerView.AdFailedToLoad -= HandleAdFailedToLoad;
		bannerView.AdOpened -= HandleAdOpened;
		bannerView.AdClosing -= HandleAdClosing;
		bannerView.AdClosed -= HandleAdClosed;
		bannerView.AdLeftApplication -= HandleAdLeftApplication;



	
		// Called when an ad request has successfully loaded.
		bannerView.AdLoaded += HandleAdLoaded;
		// Called when an ad request failed to load.
		bannerView.AdFailedToLoad += HandleAdFailedToLoad;
		// Called when an ad is clicked.
		bannerView.AdOpened += HandleAdOpened;
		// Called when the user is about to return to the app after an ad click.
		bannerView.AdClosing += HandleAdClosing;
		// Called when the user returned from the app after an ad click.
		bannerView.AdClosed += HandleAdClosed;
		// Called when the ad click caused the user to leave the application.
		bannerView.AdLeftApplication += HandleAdLeftApplication;
		}
		#endif

		#if STAN_ASSET_GOOGLEMOBILEADS
		if (!GoogleMobileAd.IsInited)
		{
		GoogleMobileAd.Init ();
		} 
		else 
		{
		GoogleMobileAdBanner banner;

		if (registerdBanners.ContainsKey (sceneBannerId))
		{
		banner = registerdBanners [sceneBannerId];
		} 
		else 
		{
		banner = GoogleMobileAd.CreateAdBanner (TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);

		registerdBanners.Add (sceneBannerId, banner);
		}

		if (banner.IsLoaded && !banner.IsOnScreen) 
		{
		banner.Show ();
		}
		}
		#endif

		#if STAN_ASSET_ANDROIDNATIVE
		if (!AndroidAdMobController.Instance.IsInited)
		{
		//			AndroidAdMobController.Init ();
		Set();
		} 
		else 
		{
		GoogleMobileAdBanner banner;

		if (registerdBanners.ContainsKey (sceneBannerId))
		{
		banner = registerdBanners [sceneBannerId];
		} 
		else 
		{
		banner = AndroidAdMobController.Instance.CreateAdBanner (TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);

		registerdBanners.Add (sceneBannerId, banner);
		}

		if (banner.IsLoaded && !banner.IsOnScreen) 
		{
		banner.Show ();
		}
		}
		#endif
	}

	private void HideBanner() 
	{
		#if STAN_ASSET_GOOGLEMOBILEADS
		if(registerdBanners.ContainsKey(sceneBannerId)) 
		{
		GoogleMobileAdBanner banner = registerdBanners[sceneBannerId];
		if(banner.IsLoaded) 
		{
		if(banner.IsOnScreen) 
		{
		banner.Hide();
		}
		} 
		else
		{
		banner.ShowOnLoad = false;
		}
		}
		#endif

		#if STAN_ASSET_ANDROIDNATIVE
		if(registerdBanners.ContainsKey(sceneBannerId)) 
		{
		GoogleMobileAdBanner banner = registerdBanners[sceneBannerId];
		if(banner.IsLoaded) 
		{
		if(banner.IsOnScreen) 
		{
		banner.Hide();
		}
		} 
		else
		{
		banner.ShowOnLoad = false;
		}
		}
		#endif
	}

	#if STAN_ASSET_GOOGLEMOBILEADS
	public static Dictionary<string, GoogleMobileAdBanner> registerdBanners 
	{
	get
	{
	if(_registerdBanners == null) 
	{
	_registerdBanners = new Dictionary<string, GoogleMobileAdBanner>();
	}

	return _registerdBanners;
	}
	}
	#endif

	#if STAN_ASSET_ANDROIDNATIVE
	public static Dictionary<string, GoogleMobileAdBanner> registerdBanners 
	{
	get
	{
	if(_registerdBanners == null) 
	{
	_registerdBanners = new Dictionary<string, GoogleMobileAdBanner>();
	}

	return _registerdBanners;
	}
	}
	#endif

	public string sceneBannerId 
	{
		get 
		{
			return Application.loadedLevelName + "_" + this.gameObject.name;
		}
	}

	#if UNITY_IOS
	void OnBannerClicked()
	{
		Debug.Log("Clicked!\n");
	}

	void OnBannerLoaded()
	{
		Debug.Log("Loaded!\n");
		banner.visible = true;
	}

	void OnBannerFailedToLoad()
	{
		Debug.Log("FAIL!\n");
		banner.visible = false;
	}

	void OnFullscreenLoaded()
	{
		// you can show ad right here, or, for example, you can start preparing your UI
		Debug.Log("AD Loaded\n");
	}
	void OnFullscreenViewed()
	{
		// If we reach this stage, it means the user viewed the Ad past the initial screen.
		// This could be a good point to reward the user (eg. give an in-game bonus item).
		// You can also start reloading the Ad here if you are not using built-in auto reloading.
		Debug.Log("AD Viewed\n");
		fullscreenAd.ReloadAd();
	}
	void WantToShowAD()
	{
		if(fullscreenAd.loaded)
			fullscreenAd.Show();
		else
			fullscreenAd.ReloadAd();
	}
	#endif

	public void ShowAdsGameOver()
	{
		int count = PlayerPrefs.GetInt ("numberOfLoseToShowInterstitial", 0);

		count++;

		if (count >= 3) 
		{
			PlayerPrefs.SetInt ("numberOfLoseToShowInterstitial", 0);
			PlayerPrefs.Save ();


			#if CHARTBOOST
		int rand = UnityEngine.Random.Range (0, 2);
		switch (rand)
		{
		case 0:
		ShowAdmobInterstitialGameOver();
		break;
		case 1:
		ShowChartboostInterstitialGameOver ();
		break;
		default:
		ShowAdmobInterstitialGameOver();
		break;
		}
			#else

			ShowAdmobInterstitialGameOver ();

			#endif
		}
		else 
		{
			PlayerPrefs.SetInt ("numberOfLoseToShowInterstitial", count);
			PlayerPrefs.Save ();

		}
	}


	public bool RewardedVideoIsInitialized()
	{

		bool adsReady = false;


		#if CHARTBOOST

		adsReady = adsReady ||Chartboost.hasRewardedVideo(CBLocation.Default);
		if (!Chartboost.hasRewardedVideo (CBLocation.Default)) 
		{
		Chartboost.cacheRewardedVideo (CBLocation.Default);
		}
		#endif

		#if UNITY_ADS
		adsReady = adsReady ||Advertisement.IsReady ("rewardedVideo");
		#endif

		if (Application.isEditor)
			adsReady = rewardedVideoAlwaysReadyInSimulator;

		return  adsReady;
	}


	public void ShowRewardedVideoGameOver(Action<bool> success)
	{
		if (Application.isEditor || !Application.isMobilePlatform) 
		{
			if (success != null)
				success (rewardedVideoAlwaysSuccessInSimulator);
		}

		#if CHARTBOOST
		if (!Application.isEditor) 
		{
		if (Chartboost.hasRewardedVideo (CBLocation.Default)) 
		{
		Chartboost.showRewardedVideo (CBLocation.Default);

		Chartboost.didCompleteRewardedVideo += delegate(CBLocation arg1, int arg2) {
		Debug.Log("!!!!!! Chartboost didCompleteRewardedVideo at location : " + arg1.ToString());

		if (success != null)
		success (true);

		};

		Chartboost.didFailToLoadRewardedVideo += delegate(CBLocation arg1, CBImpressionError arg2) {
		Debug.Log("!!!!!! Chartboost didFailToLoadRewardedVideo at location : " + arg1.ToString());

		if (success != null)
		success (true);
		};
		}
		else 
		{


		Chartboost.cacheRewardedVideo (CBLocation.Default);
		#endif
		#if UNITY_ADS
		Advertisement.Show ("rewardedVideo", new ShowOptions 
			{

				resultCallback = result => {
					if (result == ShowResult.Finished) {

						Debug.Log ("user finished unity ads ===> offer 1 coin");

						if (success != null)
							success (true);

					} else if (result == ShowResult.Failed) {

						Debug.Log ("unity ads failed : " + result.ToString ());


						if (success != null)
							success (false);

					} else if (result == ShowResult.Skipped) {

						Debug.Log ("unity ads Skipped: " + result.ToString ());


						if (success != null)
							success (false);
					}
				}
			});
		#endif
		#if CHARTBOOST
		}


		} else {

		if(success!=null)
		success(true);

		}
		#endif
	}


	void ShowAdmobInterstitialGameOver()
	{

		#if UNITY_IOS
		WantToShowAD();
		#endif


		#if GOOGLE_MOBILE_ADS
		if (Application.isMobilePlatform && interstitial.IsLoaded()) 
		{
			interstitial.Show();
		}
		else
		{
			RequestInterstitial();	
		}
		#endif

		#if STAN_ASSET_GOOGLEMOBILEADS
		GoogleMobileAd.OnInterstitialLoaded -= HandleOnInterstitialLoadedGameOver;

		GoogleMobileAd.OnInterstitialFailedLoading -= HandleOnInterstitialFailedLoadingGameOver;

		GoogleMobileAd.OnInterstitialLoaded += HandleOnInterstitialLoadedGameOver;

		GoogleMobileAd.OnInterstitialFailedLoading += HandleOnInterstitialFailedLoadingGameOver;

		GoogleMobileAd.LoadInterstitialAd ();
		#endif

		#if STAN_ASSET_ANDROIDNATIVE
		AndroidAdMobController.Instance.OnInterstitialLoaded -= HandleOnInterstitialLoadedGameOver;

		AndroidAdMobController.Instance.OnInterstitialFailedLoading -= HandleOnInterstitialFailedLoadingGameOver;

		AndroidAdMobController.Instance.OnInterstitialLoaded += HandleOnInterstitialLoadedGameOver;

		AndroidAdMobController.Instance.OnInterstitialFailedLoading += HandleOnInterstitialFailedLoadingGameOver;

		AndroidAdMobController.Instance.LoadInterstitialAd ();
		#endif
	}
	void HandleOnInterstitialLoadedGameOver () 
	{
		#if STAN_ASSET_GOOGLEMOBILEADS
		GoogleMobileAd.OnInterstitialLoaded -= HandleOnInterstitialLoadedGameOver;
		//ad loaded, strting ad
		GoogleMobileAd.ShowInterstitialAd ();
		#endif

		#if STAN_ASSET_ANDROIDNATIVE
		AndroidAdMobController.Instance.OnInterstitialLoaded -= HandleOnInterstitialLoadedGameOver;
		//ad loaded, strting ad
		AndroidAdMobController.Instance.ShowInterstitialAd ();
		#endif
	}

	void HandleOnInterstitialFailedLoadingGameOver()
	{
		#if STAN_ASSET_GOOGLEMOBILEADS
		GoogleMobileAd.OnInterstitialFailedLoading -= HandleOnInterstitialFailedLoadingGameOver;
		#endif

		#if STAN_ASSET_ANDROIDNATIVE
		AndroidAdMobController.Instance.OnInterstitialFailedLoading -= HandleOnInterstitialFailedLoadingGameOver;
		#endif

		#if CHARTBOOST
		Chartboost.showInterstitial (CBLocation.Default);
		#endif
	}


	void ShowAdmobBackup()
	{
		#if STAN_ASSET_GOOGLEMOBILEADS
		GoogleMobileAd.OnInterstitialLoaded -= HandleOnInterstitialLoadedGameOverBackUp;

		GoogleMobileAd.LoadInterstitialAd ();
		#endif

		#if STAN_ASSET_ANDROIDNATIVE
		AndroidAdMobController.Instance.OnInterstitialLoaded -= HandleOnInterstitialLoadedGameOverBackUp;

		AndroidAdMobController.Instance.LoadInterstitialAd ();
		#endif
	}

	void HandleOnInterstitialLoadedGameOverBackUp ()
	{
		#if STAN_ASSET_GOOGLEMOBILEADS
		GoogleMobileAd.OnInterstitialLoaded -= HandleOnInterstitialLoadedGameOverBackUp;
		GoogleMobileAd.ShowInterstitialAd ();
		#endif

		#if STAN_ASSET_ANDROIDNATIVE
		AndroidAdMobController.Instance.OnInterstitialLoaded -= HandleOnInterstitialLoadedGameOverBackUp;
		AndroidAdMobController.Instance.ShowInterstitialAd ();
		#endif
	}


	#if CHARTBOOST

	void ShowChartboostInterstitialGameOver()
	{
	Chartboost.didFailToLoadInterstitial -= didFailedToLoaChartboostGameOver;
	Chartboost.didFailToLoadInterstitial += didFailedToLoaChartboostGameOver;

	Chartboost.showInterstitial (CBLocation.Default);
	}

	void didFailedToLoaChartboostGameOver(CBLocation location, CBImpressionError error)
	{
	Chartboost.didFailToLoadInterstitial -= didFailedToLoaChartboostGameOver;

	if (location != CBLocation.Startup) {
	Debug.Log(string.Format("--------- didFailedToLoaChartboostGameOver didFailToLoadInterstitial: {0} at location {1}", error, location));
	} else {
	Debug.Log(string.Format("---------!!!! ERROR !!!!  didFailedToLoaChartboostGameOver didFailToLoadInterstitial: {0} at location {1}", error, location));
	}
	}

	void didFailToLoadInterstitialGameOver(CBLocation location, CBImpressionError error) {
	Debug.Log(string.Format("CHARTBOOST ----- didFailToLoadInterstitial: {0} at location {1}", error, location));

	if (location == CBLocation.Startup) {
	Debug.Log("it's startup ==> on base");
	} else {
	Debug.Log("it's NOT startup ==> on tente une autre ad");

	ShowAdmobBackup ();

	}

	}
	#endif

	void OnDestroy() 
	{
		HideBanner();
	}


//	[SerializeField] private Transform popupContinueParent;
//	[SerializeField] private Image popupContinueFillImage;
//	[SerializeField] private Button buttonPopupContinue;
//	float speedFillImage = 2f;
//	float  speedScaleParent = 0.5f;
//	public bool m_OnClickedContinue;

//	private float fillAmountPopupContinue
//	{
//		get 
//		{
//			return popupContinueFillImage.fillAmount;
//		}
//
//		set 
//		{
//			popupContinueFillImage.fillAmount = value;
//		}
//	}
//
//	private float scalePopupContinueParent
//	{
//		get 
//		{
//			return popupContinueParent.localScale.x;
//		}
//
//		set 
//		{
//			popupContinueParent.localScale = Vector3.one*value;
//		}
//	}
//
//	public void OnClickedContinue()
//	{
//		m_OnClickedContinue = true;
//
//		buttonPopupContinue.onClick.RemoveListener (OnClickedContinue);
//	}
//
//	public void OpenPopupContinue (Action<bool> onClick)
//	{
//		m_OnClickedContinue = false;
//
//		popupContinueParent.gameObject.SetActive (true);
//
//		scalePopupContinueParent = 0f;
//
//		fillAmountPopupContinue = 1f;
//
//
//		StartCoroutine (DoLerpImage (onClick));
//	}
//
//	IEnumerator DoLerpImage(Action<bool> onClick)
//	{
//		m_OnClickedContinue = false;
//
//		float timer = 0;
//		while (timer <= speedScaleParent)
//		{
//			timer += Time.deltaTime;
//			scalePopupContinueParent = Mathf.Lerp (0f, 1f, timer / speedScaleParent);
//			yield return null;
//		}
//
//		buttonPopupContinue.onClick.AddListener (OnClickedContinue);
//
//
//		timer = 0;
//		while (timer <= speedFillImage)
//		{
//			timer += Time.deltaTime;
//			fillAmountPopupContinue = Mathf.Lerp (1f, 0f, timer / speedFillImage);
//			yield return null;
//
//			if (m_OnClickedContinue)
//				break;
//		}
//
//		timer = 0;
//		while (timer <= speedScaleParent)
//		{
//			timer += Time.deltaTime;
//			scalePopupContinueParent = Mathf.Lerp (1, 0f, timer / speedScaleParent);
//
//			yield return 0;
//
//			if (m_OnClickedContinue)
//				break;
//		}
//
//		if (m_OnClickedContinue) 
//		{
//
//			timer = 0;
//			while (timer <= speedScaleParent)
//			{
//				timer += Time.deltaTime;
//				scalePopupContinueParent = Mathf.Lerp (1, 0f, timer / speedScaleParent);
//
//				yield return 0;
//			}
//
//
//			popupContinueParent.gameObject.SetActive (false);
//
//			scalePopupContinueParent = 0f;
//
//			fillAmountPopupContinue = 1f;
//		}
//
//		if (onClick != null)
//			onClick (m_OnClickedContinue);
//	}
}
