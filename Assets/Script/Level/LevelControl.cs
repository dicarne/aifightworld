using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	public void ResetHero()
	{
		var hero = GamobjList.First(dinfo => dinfo.Name == "myhero");
		if (hero)
		{
			hero.transform.localPosition = new Vector3(-1.2f, 0, 0);
		}
		
	}
}
