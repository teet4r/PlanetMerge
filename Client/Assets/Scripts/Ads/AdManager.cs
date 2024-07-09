using GoogleMobileAds.Api;
using System;
using UnityEngine;

public static class AdManager
{
    public static void Initialize()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) => {
            // This callback is called once the MobileAds SDK is initialized.
        });

        _interstitialAd = null;
        _bannerView = null;

        _LoadInterstitialAd();
    }

    /// <summary>
    /// 전면 광고
    /// </summary>
    private static InterstitialAd _interstitialAd;

#if UNITY_EDITOR
    private static string _interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_ANDROID
    private static string _interstitialAdUnitId = "ca-app-pub-7910487525826129/4858832900";
#endif

    private static void _LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        InterstitialAd.Load(_interstitialAdUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    _HandleError(error);
                    return;
                }

                _interstitialAd = ad;
            }
        );
    }

    public static void ShowInterstitialAd(Action onClosed)
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _interstitialAd.OnAdFullScreenContentClosed += onClosed;
            _interstitialAd.Show();
        }
        else
            _LoadInterstitialAd();
    }





    /// <summary>
    /// 배너 광고
    /// </summary>
    private static BannerView _bannerView;

    public static float BannerHeight => _bannerView.GetHeightInPixels();
    public static float BannerWidth => _bannerView.GetWidthInPixels();

#if UNITY_EDITOR
    private static string _bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_ANDROID
    private static string _bannerAdUnitId = "ca-app-pub-7910487525826129/4241478123";
#endif

    /// <summary>
    /// Creates a 320x50 banner view at top of the screen.
    /// </summary>
    private static void _CreateBannerView()
    {
        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
            DestroyBannerView();

        // Create a 320x50 banner at top of the screen
        //AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        _bannerView = new BannerView(_bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) => _HandleError(error);
    }

    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public static void LoadBannerAd()
    {
        // create an instance of a banner view first.
        if (_bannerView == null)
            _CreateBannerView();

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        _bannerView.LoadAd(adRequest);
    }

    /// <summary>
    /// Destroys the banner view.
    /// </summary>
    public static void DestroyBannerView()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
            _bannerView = null;
        }
    }



    private static void _HandleError(LoadAdError error)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            UIManager.Get<UIRemindPopup>()
                .SetTitle(Translator.Get("$$에러"))
                .SetDescription(Translator.Get("$$인터넷이 연결되어 있지 않습니다."))
                .SetFirstButton(Translator.Get("$$종료"), () =>
                {
                    Application.Quit();
                })
                .Show();
        }
        else
        {
            UIManager.Get<UIRemindPopup>()
                .SetTitle(Translator.Get("$$에러"))
                .SetDescription(Translator.Get("$$에러가 발생하였습니다: {0}", error.GetMessage()))
                .SetFirstButton(Translator.Get("$$종료"), () =>
                {
                    Application.Quit();
                })
                .Show();
        }
    }
}
