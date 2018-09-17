using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XLua;


public class SceneMang : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BackToMenu()
	{
		SceneManager.LoadScene("Menu");
	}

	public void StartLevel()
	{
		SceneManager.LoadScene("Battle");
	}

	public void Exit()
	{
		Application.Quit();
	}

}

public static class black
{
	[BlackList]
	public static List<List<string>> BlackList = new List<List<string>>()  {
		new List<string>(){"UnityEngine.Light", "shadowAngle"},
		new List<string>(){"UnityEngine.Light", "shadowRadius"},
	};
}