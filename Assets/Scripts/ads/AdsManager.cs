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
                AdsManager._instance = new AdsManager();

            return AdsManager._instance;
        }
    }

    private InterstitialAd _interstitial;

    private bool _initializeWasCalled = false;

    public void Init()
    {
        if (this._initializeWasCalled) return;

        this._initializeWasCalled = true;

        this._InitAdMob();
        this._InitIronSource();
        this._ConnectToTenjin();
    }

    public void OnApplicationPause(bool isPaused) 
    {                 
        IronSource.Agent.onApplicationPause(isPaused);

        if (! isPaused) this._ConnectToTenjin();
    }

    private void _InitAdMob()
    {
        Debug.Log("MobileAds.Initialize Called");
        MobileAds.Initialize(initStatus => {
            Debug.Log("MobileAds.Initialize Finished");
            this._RequestInterstitial();
        });
    }

    private void _InitIronSource()
    {
        Debug.Log("Ironsource.Agent.init called");
        IronSource.Agent.init(AppConfig.ironSourceAppKey);

        //Debug.Log("Ironsource.Agent.validateIntegration called");
        //IronSource.Agent.validateIntegration();
    }

    private void _ConnectToTenjin()
    {
        Debug.Log("Tenjin Connect called");
        BaseTenjin instance = Tenjin.getInstance(AppConfig.tenjinAppKey);
        instance.Connect();
    }
    
    public void ShowInterstitialAndThen(System.Action callback)
    {
        #if UNITY_EDITOR
            callback?.Invoke();
        #else
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
        #endif
    }

    private void _RequestInterstitial()
    {
        // Initialize an InterstitialAd.
        this._interstitial = new InterstitialAd(AppConfig.adUnitId);

        // Create an empty ad request.
        var builder = new AdRequest.Builder();
        foreach (var id in AppConfig.testDevicesIds)
        {
            builder.AddTestDevice(id);
        }

        // Load the interstitial with the request.
        this._interstitial.OnAdLoaded += (sender, args) => {
            Debug.Log("InterstitialAd OnAdLoaded");
        };

        Debug.Log("InterstitialAd.LoadAd Called");
        this._interstitial.LoadAd(builder.Build());
    }
}