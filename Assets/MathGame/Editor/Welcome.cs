using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Welcome : EditorWindow
{
	bool groupEnabled;
	private GUIStyle welcomeStyle = null;
	// Add menu item named "My Window" to the Window menu
	[MenuItem("Window/MATH GAME FIRST START INFORMATIONS")]
	public static void Initialize () 
	{

		// Get existing open window or if none, make a new one:
		Welcome window = (Welcome)EditorWindow.GetWindow (typeof (Welcome), true, "PLEASE HAVE A LOOK TO THE DOCUMENTATION");


		GUIStyle style = new GUIStyle();

		window.position = new Rect(196, 196, sizeWidth, sizeHeight);
		window.minSize = new Vector2(sizeWidth, sizeHeight);
		window.maxSize = new Vector2(sizeWidth, sizeHeight);
		window.welcomeStyle = style;
		window.Show();

	}

	static float sizeWidth = 630;
	static float sizeHeight = 650;
	void OnGUI()
	{
		if(welcomeStyle == null)
			return;

		if (GUI.Button(new Rect(10, 10, 300, 150), "OPEN MATH GAME DOCUMENTATION"))
		{
			Application.OpenURL("https://dl.dropboxusercontent.com/u/8289407/MathGameAssetStore/_ReadMe.txt");
		}

		if (GUI.Button(new Rect(10, 150 + 20, 300, 150), "OPEN ADS INTEGRATION DOCUMENTATION"))
		{
			Application.OpenURL("https://dl.dropboxusercontent.com/u/8289407/MathGameAssetStore/_Ads_Integration_Documentation.pdf");
		}

		if (GUI.Button(new Rect(10, 300 + 30, 300, 150), "VIDEO TUTORIAL"))
		{
			Application.OpenURL("https://youtu.be/Ba1vKG6deh4");
		}

		if (GUI.Button(new Rect(20 + 300, 10, 300, 150), "FORUM THREAD"))
		{
			Application.OpenURL("http://forum.unity3d.com/threads/math-game-brain-workout-complete-game-ready-for-release.376318/");
		}

		if (GUI.Button(new Rect(20 + 300, 150 + 20, 300, 150), "RATE THIS ASSET"))
		{
			Application.OpenURL("http://u3d.as/mAb");
		}

		if (GUI.Button(new Rect(20 + 300, 300 + 30, 300, 150), "MORE ASSETS FROM US"))
		{
			Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:8911");
		}

		if (GUI.Button(new Rect(10, 450 + 40, 610, 150), "DON'T PROMPT ME AGAIN"))
		{
			DoClose();
			PlayerPrefs.SetInt("DocumentationOpened",1);
			PlayerPrefs.Save();
		}
	}

	void DoClose()
	{
		Close();
	}
}

[InitializeOnLoad]
class StartupHelper
{
	static StartupHelper()
	{
		EditorApplication.update += Startup;
	}
	static void Startup()
	{

		if(Time.realtimeSinceStartup < 1)
			return;
		
		EditorApplication.update -= Startup;

		if (!PlayerPrefs.HasKey("DocumentationOpened"))
		{
			Welcome.Initialize();
		}
	}
} 

