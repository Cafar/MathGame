using UnityEngine;
using System.Collections;

public class ButtonLeaderboard : ButtonHelper 
{
	override public void OnClicked()
	{
		print ("OnClicked : " + gameObject.name);
	}
}

