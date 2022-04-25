using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdManager instance;
    [SerializeField] string AndroidGameID, IOSGameID, androidAdUnitId = "Interstitial_Android", IOSAdUnitId = "Interstitial_iOS";
    [SerializeField] bool testMode = false, adLoaded = false, showAd = false;
    private string gameID, adUnityID;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        //Setup initialization and Interstatial Ad ID according to platform.
        if (Application.platform == RuntimePlatform.Android)
        {
            gameID = AndroidGameID;
            adUnityID = androidAdUnitId;
        } else {
            gameID = IOSGameID;
            adUnityID = IOSAdUnitId;
        }
        Advertisement.Initialize(gameID, testMode);
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization.
        Debug.Log("Loading Ad: " + adUnityID);
        Advertisement.Load(adUnityID, this);
        adLoaded = true;
    }

    // Show the loaded content in the Ad Unit: 
    public void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        if (adLoaded)
        {
            Debug.Log("Showing Ad: " + adUnityID);
            Advertisement.Show(adUnityID, this);
            showAd = false;
        } else {
            LoadAd();
        }
    }

    // Implement Load Listener and Show Listener interface methods:  
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
        if (showAd)
        {
            Debug.Log("Ad " + adUnityID + " loaded.");
            ShowAd();
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) {
        Debug.Log("Ad showing");
        Time.timeScale = 0;
    }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) { 
        Debug.Log("Ad complete");
        Time.timeScale = 1;
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}