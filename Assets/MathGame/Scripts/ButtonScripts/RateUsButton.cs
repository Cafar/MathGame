using UnityEngine;
using System.Collections;

public class RateUsButton : ButtonHelper 
{
	override public void OnClicked()
	{
		print ("OnClicked : " + gameObject.name);

	}
}
