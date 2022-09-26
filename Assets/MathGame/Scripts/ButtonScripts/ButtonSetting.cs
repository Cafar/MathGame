using UnityEngine;
using System.Collections;

public class ButtonSetting : ButtonHelper 
{
	override public void OnClicked()
	{
		print ("OnClicked : " + gameObject.name);
		menuManager.OpenSettings ();
		RemoveListener();
	}
}