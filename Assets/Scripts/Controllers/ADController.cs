//using GoogleMobileAds.Api;
//using GoogleMobileAds.Common;
using UnityEngine.Advertisements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ADController : MonoBehaviour, IUnityAdsListener
{
    public static ADController instance;

#if UNITY_IOS
    [SerializeField] string gameId = "4171618";
#elif UNITY_ANDROID
    [SerializeField] string gameId = "4171619";
#endif
     
    [SerializeField] string myPlacementId = "interstitialAd";
    [SerializeField] bool testMode = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementId)
        {
            Debug.Log("Ads Ready!");
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            Debug.LogWarning("Finished");
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            Debug.LogWarning("Skipped");
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }


//#if UNITY_ANDROID
//    [SerializeField] string interstitialKey = "ca-app-pub-3875814216549155/2832717057";
//#elif UNITY_IPHONE
//    [SerializeField] string interstitialKey = "ca-app-pub-3875814216549155/2832717057";
//#else
//    [SerializeField] string interstitialKey = "unexpected_platform";
//#endif

//    private InterstitialAd interstitial;

//    public UnityEvent OnAdLoadedEvent;
//    public UnityEvent OnAdFailedToLoadEvent;
//    public UnityEvent OnAdOpeningEvent;
//    public UnityEvent OnAdFailedToShowEvent;
//    public UnityEvent OnUserEarnedRewardEvent;
//    public Action OnAdClosedEvent;

//    public static void Initialize(string gameId, bool testMode, bool enablePerPlacementLoad)
//    {

//    }

//    private void Awake()
//    {
//        if (instance == null)
//            instance = this;
//        else
//            Destroy(gameObject);

//    }

//    private void Start()
//    {
//        // Initialize the Google Mobile Ads SDK.
//        //MobileAds.Initialize(initStatus => 
//        //{
//        //    RequestInterstitial();
//        //});

//        MobileAds.Initialize(HandleInitCompleteAction);
//    }

//    private void HandleInitCompleteAction(InitializationStatus initstatus)
//    {
//        // Callbacks from GoogleMobileAds are not guaranteed to be called on
//        // main thread.
//        // In this example we use MobileAdsEventExecutor to schedule these calls on
//        // the next Update() loop.
//        MobileAdsEventExecutor.ExecuteInUpdate(() =>
//        {
//            //statusText.text = "Initialization complete";
//            RequestInterstitial();
//        });
//    }

//    #region HELPER METHODS

//    private AdRequest CreateAdRequest()
//    {
//        return new AdRequest.Builder()
//            .AddKeyword("unity-admob-sample")
//            .Build();
//    }

//    #endregion

//    #region Interstitial
//    private void RequestInterstitial()
//    {
//        if (Debug.isDebugBuild)
//            interstitialKey = "ca-app-pub-3940256099942544/1033173712";

//        if (interstitial != null)
//        {
//            interstitial.Destroy();
//        }
//        // Initialize an InterstitialAd.
//        this.interstitial = new InterstitialAd(interstitialKey);

//        // Called when an ad request has successfully loaded.
//        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
//        // Called when an ad request failed to load.
//        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
//        // Called when an ad is shown.
//        this.interstitial.OnAdOpening += HandleOnAdOpened;
//        // Called when the ad is closed.
//        this.interstitial.OnAdClosed += HandleOnAdClosed;

//        // Create an empty ad request.
//        AdRequest request = new AdRequest.Builder().Build();
//        // Load the interstitial with the request.
//        this.interstitial.LoadAd(request);
//    }

//    public void HandleOnAdLoaded(object sender, EventArgs args)
//    {
//        MonoBehaviour.print("HandleAdLoaded event received");
//    }

//    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
//    {
//        print("Interstitial failed to load: " + args.LoadAdError.GetMessage());
//        // Handle the ad failed to load event.
//    }

//    public void HandleOnAdOpened(object sender, EventArgs args)
//    {
//        MonoBehaviour.print("HandleAdOpened event received");
//    }

//    public void HandleOnAdClosed(object sender, EventArgs args)
//    {
//        MonoBehaviour.print("HandleAdClosed event received");
//        OnAdClosedEvent?.Invoke();
//        AdRequest request = new AdRequest.Builder().Build();
//        // Load the interstitial with the request.
//        this.interstitial.LoadAd(request);
//    }

//    #endregion

public void ShowInterstitial()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady())
        {
            Advertisement.Show(myPlacementId);
            // Replace mySurfacingId with the ID of the placements you wish to display as shown in your Unity Dashboard.
        }
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }
}
