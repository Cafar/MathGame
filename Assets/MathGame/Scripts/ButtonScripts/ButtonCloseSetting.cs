using UnityEngine;
using System.Collections;

public class ButtonCloseSetting : ButtonHelper 
{
	override public void OnClicked()
	{
		print ("OnClicked : " + gameObject.name);
		menuManager.CloseSettings ();
		RemoveListener();
	}
}