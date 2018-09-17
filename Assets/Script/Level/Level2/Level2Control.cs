using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level2Control : MonoBehaviour
{
	public GameObject tile;

	public GameObject root;

	void Awake()
	{
		Players = new List<TileHeroInfo>();
		Map = new Dictionary<Vector2Int, TileCtrl>();
	}
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

		Map[new Vector2Int(0, 0)].Player = TileCtrl.EPlayer.Player0;
		Map[new Vector2Int(0, 0)].Type = TileCtrl.EType.Building;
		Map[new Vector2Int(9, 9)].Player = TileCtrl.EPlayer.Player1;
		Map[new Vector2Int(9, 9)].Type = TileCtrl.EType.Building;


		StartCoroutine(Turn());
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
	
	public int GetScore(TileCtrl.EPlayer player)
	{
		var sum = 0;
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				var tile = Map[new Vector2Int(i, j)];
				if (tile.Player == player)
				{
					switch (tile.Type)
					{
						case TileCtrl.EType.Land:
							break;
						case TileCtrl.EType.Building:
							sum += tile.BuildingLevel * 10;
							break;
						case TileCtrl.EType.Factory:
							break;
						case TileCtrl.EType.Home:
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}
		}

		return sum;
	}

	private static Vector2Int[] dir =
	{
		new Vector2Int(0, -1),
		new Vector2Int(-1, 0),
		new Vector2Int(1, 0),
		new Vector2Int(0, 1),
	};

	public Text TextRed;
	public Text TextBlue;
	public Text TextTurn;
	private int turnnum = 0;
	IEnumerator Turn()
	{
		var AnotherGameObj = GameObject.Find("Player1").GetComponent<TileHero>();
		var TargetGameObj = GameObject.Find("Player0").GetComponent<TileHero>();
		while (true)
		{
			TextTurn.text = turnnum.ToString();
			turnnum++;
			TargetGameObj.OnTurn();
			TextBlue.text = GetScore(TileCtrl.EPlayer.Player0).ToString();
			yield return new WaitForSeconds(0.2f);
			AnotherGameObj.OnTurn();
			TextRed.text = GetScore(TileCtrl.EPlayer.Player1).ToString();
			yield return new WaitForSeconds(0.2f);
			foreach (var tl in Map)
			{
				if(tl.Value.Player!=TileCtrl.EPlayer.No) continue;
				int countA = 0;
				int countB = 0;
				for (int i = 0; i < 4; i++)
				{
					TileCtrl t;
					if (Map.TryGetValue(tl.Value.pos + dir[i], out t))
					{
						if (t.Player == TileCtrl.EPlayer.Player0) countA++;
						if (t.Player == TileCtrl.EPlayer.Player1) countB++;
					}
				}

				if (countA == 4)
				{
					tl.Value.Player = TileCtrl.EPlayer.Player0;
					tl.Value.BuildingLevel = 3;
					tl.Value.Type = TileCtrl.EType.Building;
				}else if (countB == 4)
				{
					tl.Value.Player = TileCtrl.EPlayer.Player1;
					tl.Value.BuildingLevel = 3;
					tl.Value.Type = TileCtrl.EType.Building;
				}
			}
		}
	}
}

