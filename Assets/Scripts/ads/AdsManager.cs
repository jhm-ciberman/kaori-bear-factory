using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager
{
    private static AdsManager _instance = null;

    public static AdsManager instance
    {
        get
        {
            if (AdsManager._instance == null)
            {
                AdsManager._instance = new AdsManager();
            }

            return AdsManager._instance;
        }
    }

    private InterstitialAd _interstitial;

    private bool _initializeWasCalled = false;

    public void Init()
    {
        if (this._initializeWasCalled) return;

        this._initializeWasCalled = true;
        // Initialize the Google Mobile Ads SDK.
        Debug.Log("MobileAds.Initialize Called");
        MobileAds.Initialize(initStatus => {
            Debug.Log("MobileAds.Initialize Finished");
            this._RequestInterstitial();
        });
    }
    
    public void ShowInterstitialAndThen(System.Action callback)
    {
        if (this._interstitial != null && this._interstitial.IsLoaded())
        {
            this._interstitial.OnAdClosed += (sender, args) => callback?.Invoke();
            this._interstitial.Show();
            this._interstitial = null;

            this._RequestInterstitial();
        }
        else
        {
            callback?.Invoke();
        }
    }

    private void _RequestInterstitial()
    {
        #if UNITY_EDITOR
            string adUnitId = "unused";
        #elif UNITY_ANDROID
            //string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // Test Ad
            string adUnitId = "ca-app-pub-8938056874619269/2918817667"; // Real Ad
        #elif UNITY_IPHONE
            string adUnitId = "unexpected_platform";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Initialize an InterstitialAd.
        this._interstitial = new InterstitialAd(adUnitId);

        // Create an empty ad request.
        var builder = new AdRequest.Builder();
        builder.AddTestDevice("6AFA163589E44B52902FF87313B07633");

        // Load the interstitial with the request.
        this._interstitial.OnAdLoaded += (sender, args) => {
            Debug.Log("InterstitialAd OnAdLoaded");
        };

        Debug.Log("InterstitialAd.LoadAd Called");
        this._interstitial.LoadAd(builder.Build());
    }
}