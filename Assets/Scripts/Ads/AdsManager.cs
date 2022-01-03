using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;
using UnityEngine.Analytics;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
  // #if UNITY_IOS
        string gameId = "4527874";
        string interstitial = "Interstitial_iOS";
        string rewarded = "Rewarded_iOS";
    // #else
    //     string gameId = "4527875";
    //     string interstitial = "Interstitial_Android";
    //     string rewarded = "Rewarded_Android";
  //  #endif

    public static AdsManager Instance;
    public event EventHandler OnInterstitialAdFinished;
    public event EventHandler OnRewardedAdFinished;

    void Awake() {
        if(Instance != null) {
            return;
        }
        
        Instance = this;
    }
   
    void Start()
    {
        Advertisement.Initialize(gameId);
        Advertisement.AddListener(this);
    }

  
    public void PlayInterstitialAd() { //doesnt play ad to continue if its the first clic of the game, after that plays ad

        if(PlayerController2.Instance.continueCostIndex > 0) {
            if(Advertisement.IsReady(interstitial)) {
            Advertisement.Show(interstitial);
            }
        }
        else
        OnInterstitialAdFinished?.Invoke(this, EventArgs.Empty);
        
    }

    public void PlayRewardedAd() {
        if(Advertisement.IsReady(rewarded)) {
            Advertisement.Show(rewarded);
        }
        else {
            Debug.Log("Rewarded Ad is not ready!");
        }
    }

    public void OnUnityAdsReady(string placementId) {
      //  Debug.Log("ADS are ready");
    }
    public void OnUnityAdsDidError(string message) {
        Debug.Log("Error " + message);
    }
    public void OnUnityAdsDidStart(string placementId) {
        Debug.Log("video started");
    }
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        if(placementId == rewarded && showResult == ShowResult.Finished) {
            Debug.Log("reward player");
            OnRewardedAdFinished?.Invoke(this, EventArgs.Empty);
        }
        else if(placementId == interstitial) {
            if(showResult == ShowResult.Finished || showResult == ShowResult.Skipped)
                OnInterstitialAdFinished?.Invoke(this, EventArgs.Empty);
        }
        
            
    }
    


}
