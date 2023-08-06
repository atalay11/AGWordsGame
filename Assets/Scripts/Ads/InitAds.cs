using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InitAds : MonoBehaviour, IUnityAdsInitializationListener
{
    public string androidGameID;
    public string iosGameID;

    public bool isTestingMode = true;

    private string gameID;

    void Awake()
    {
        InitializeAdvertisement();
    }

    void InitializeAdvertisement()
    {

#if UNITY_IOS
        gameId = iosGameId;
#elif UNITY_ANDROID
        gameID = androidGameID;
#elif UNITY_EDITOR
        gameId = androidGameId;//for testing
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameID, isTestingMode, this);//ONLY ONCE
        }
        else
        {
            Debug.LogError("Advertisement is neither initialized or isSupported");
        }

    }

    public void OnInitializationComplete()
    {
        Debug.Log("Ads initialized!!");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("failed to initialize!!");
    }
}