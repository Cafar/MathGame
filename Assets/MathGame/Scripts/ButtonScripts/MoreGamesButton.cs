using UnityEngine;
using System.Collections;

public class MoreGamesButton : ButtonHelper 
{
	string URL = "http://app-advisory.com";

	override public void OnClicked()
	{
		print ("OnClicked : " + gameObject.name);
		FindObjectOfType<AdsManager> ().ShowRewardedVideoGameOver ((bool success) => {
			print("add your own code here if you want to offer something to the player");
		});
	}
}
