// InitializeAds
using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAds : MonoBehaviour
{
	private string gameId = "3843751";

	private string videoAd = "video";

	private string rewardAd = "rewardedVideo";

	private bool forCoins;

	public static InitializeAds Instance
	{
		get;
		private set;
	}

	private void Start()
	{
		Instance = this;
		Advertisement.Initialize(gameId, false);
        if (Advertisement.IsReady("banner"))
        {
			Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
			Advertisement.Banner.Show("banner");
        }

	}

	public void ShowAd()
	{
		if (Advertisement.IsReady("video"))
		{
			Advertisement.Show("video");
		}
	}

	public void ShowRewardVideoAd(string name)
    {
		if(name == "for coins")
        {
			ShowRewardedVideo("rewardedVideo2");
			forCoins = true;
        }
		else if(name == "for revive")
        {
			forCoins = false;
			ShowRewardedVideo("rewardedVideo");
		}
    }
	public void ShowRewardedVideo(string Placementid)
	{
		ShowOptions showOptions = new ShowOptions();
		showOptions.resultCallback = HandleShowResult;
		Advertisement.Show(Placementid, showOptions);
		GameManager.Instance.ChangeGameState(GameManager.GameState.WatchingAd);
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("Video completed - Offer a reward to the player");
			RewardPlayer();
			break;
		case ShowResult.Skipped:
			Debug.LogWarning("Video was skipped - Do NOT reward the player");
			break;
		case ShowResult.Failed:
			Debug.LogError("Video failed to show");
			break;
		}
	}

	private void RewardPlayer()
	{
        if (forCoins)
        {
			GameManager.Instance.CoinsReward();
		}
        else
        {
			GameManager.Instance.Revive();
		}
		
	}
}
