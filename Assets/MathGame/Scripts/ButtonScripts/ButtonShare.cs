using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MenuBarouch;

public class ButtonShare : ButtonHelper 
{
	override public void OnClicked()
	{
		print ("OnClicked : " + gameObject.name);
		RemoveListener();
	}
}
