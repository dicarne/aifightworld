using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptMang : MonoBehaviour {
	public InputField InputField;

	public LuaRunner TargetGameObj;
	
	// Use this for initialization
	void Start ()
	{
		TargetGameObj = GameObject.Find("hero").GetComponent<LuaRunner>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EndEdit()
	{
		if (TargetGameObj)
		{
			TargetGameObj.ChangeCode(InputField.text);
			Debug.Log("success");
		}
		else
		{
			Debug.Log("failed");
		}
	}
}
