using Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : SingletonBehaviour<AdManager>
{
    private static InterstitialAd _interstitialAd;

//#if UNITY_EDITOR
    private static string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
//#elif UNITY_ANDROID              
//    private static string _adUnitId = "ca-app-pub-7910487525826129/4858832900";
//#endif

    protected override void Awake()
    {
        base.Awake();

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) => {
            // This callback is called once the MobileAds SDK is initialized.
        });
    }

    private void Start()
    {
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
        InterstitialAd.Load(_adUnitId, adRequest,
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
}
