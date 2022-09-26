using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayButton : ButtonHelper 
{
	override public void OnClicked()
	{
//		print ("OnClicked : " + gameObject.name);
//		menuManager.GoToGame();
//		RemoveListener();

		SceneManager.LoadScene("Arcade");
	}
}
