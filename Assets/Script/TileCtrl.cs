using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCtrl : MonoBehaviour {
	public enum EPlayer
	{
		No,
		Player0,
		Player1
	}

	public enum EType
	{
		Land,
		Building,
		Factory,
		Home
	}
	public EPlayer Player { get; set; }
	public EType Type { get; set; }
	public Vector2Int pos;

	private Level2Control _level2Control;
	// Use this for initialization
	void Start ()
	{
		_level2Control = GameObject.Find("Manager").GetComponent<Level2Control>();
	}

	public EPlayer HeroOnIt { get; private set; } = EPlayer.No;

	// Update is called once per frame
	void Update () {
		foreach (var player in _level2Control.Players)
		{
			if (player.Hero.pos == pos)
			{
				HeroOnIt = player.Hero.Player;
			}
			else
			{
				HeroOnIt = EPlayer.No;
			}
		}
	}
	
	
}

