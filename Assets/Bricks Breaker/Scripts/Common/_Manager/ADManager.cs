using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class ADManager : Singleton<ADManager>, IUnityAdsListener
{
    string gameId = "1234567";
    string myPlacementId = "rewardedVideo";
    bool testMode = true;

    /// <summary>
    /// Initialize the Ads listener and service
    /// </summary>
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }


    private Action<ShowResult> action;

    public bool IsReady
    {
        get
        {
            return Advertisement.IsReady();
        }
    }
    
    
    
    /// <summary>
    /// Interstitial display ads
    /// </summary>
    public void ShowInterstitialAd() {
        if (Advertisement.IsReady()) {
            Advertisement.Show();
        } 
        else {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }
    
    
    /// <summary>
    /// Rewarded video ads
    /// </summary>
    public void ShowRewardedVideo(Action<ShowResult> action)
    {
        this.action = action;

        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(myPlacementId))
        {
            Advertisement.Show(myPlacementId);
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    /// <summary>
    /// Implement IUnityAdsListener interface methods
    /// </summary>
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            action(ShowResult.Finished);
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            action(ShowResult.Skipped);
        }
        else if (showResult == ShowResult.Failed)
        {
            action(ShowResult.Failed);
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == myPlacementId)
        {
            // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
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

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}


