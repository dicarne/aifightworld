using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
	public List<IDinfo> GamobjList;

	public void AssignObj(IDinfo _gameObject)
	{
		GamobjList.Add(_gameObject);
	}

	public void RemoveObj(IDinfo _gameObject)
	{
		for (var i = 0; i < GamobjList.Count; i++)
		{
			if (GamobjList[i] != _gameObject) continue;
			GamobjList.RemoveAt(i);
			break;
		}
	}
}
