using GoogleMobileAds.Api;
using System;

public static class AdManager
{
    /// <summary>
    /// Àü¸é ±¤°í
    /// </summary>
    private static InterstitialAd _interstitialAd;

#if UNITY_EDITOR
    private static string _interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_ANDROID
    private static string _interstitialAdUnitId = "ca-app-pub-7910487525826129/4858832900";
#endif

    public static void Initialize()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) => {
            // This callback is called once the MobileAds SDK is initialized.
        });

        _LoadInterstitialAd();
    }

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
                    return;

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
    /// ¹è³Ê ±¤°í
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
}
