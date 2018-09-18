using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptMang : MonoBehaviour {
	public InputField InputField;
	public LuaRunner AnotherGameObj;

	public LuaRunner TargetGameObj;
	
	// Use this for initialization
	void Start ()
	{
		AnotherGameObj = GameObject.Find("Player1").GetComponent<LuaRunner>();
		TargetGameObj = GameObject.Find("Player0").GetComponent<LuaRunner>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// 代码框结束编辑
	/// </summary>
	public void EndEdit()
	{
		if (TargetGameObj)
		{
			TargetGameObj.ChangeCode(InputField.text);
			//AnotherGameObj.ChangeCode(InputField.text);
			Debug.Log("success");
		}
		else
		{
			Debug.Log("failed");
		}
	}
	

	
}
