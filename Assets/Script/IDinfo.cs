using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDinfo : MonoBehaviour
{
	public string Name;

	public int Uid;

	public string Type;
	
	// Use this for initialization
	
	protected LevelControl LevelControl;
	void Start () {
		LevelControl = GameObject.Find("Manager").GetComponent<LevelControl>();
		LevelControl.AssignObj(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
