using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Control : MonoBehaviour
{
	public GameObject tile;

	public GameObject root;
	// Use this for initialization
	void Start () {
		for (var i = 0; i < 10; i++)
		{
			for (var j = 0; j < 10; j++)
			{
				var obj = Instantiate(tile, new Vector3(i * 20.0f - 250.0f, j * 20.0f - 30.0f, 0),
					new Quaternion(0, 0, 0, 0), root.transform);
				Map[new Vector2Int(i, j)] = obj.GetComponent<TileCtrl>();
				Map[new Vector2Int(i, j)].pos = new Vector2Int(i, j);
			}
		}
		
	}

	public Dictionary<Vector2Int, TileCtrl> Map;

	public struct TileHeroInfo
	{
		public TileCtrl.EPlayer Player;
		public TileHero Hero;
	}
	public List<TileHeroInfo> Players;
	
	// Update is called once per frame
	void Update () {
		
	}
}

