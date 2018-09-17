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

	public EPlayer Player;
	public EType Type;
	public Vector2Int pos;
	public int BuildingLevel = 1;
	public Sprite NoPlayer;
	public Sprite[] RedSprites;
	public Sprite[] BlueSprites;
	private Level2Control _level2Control;
	public bool Lock = false;

	public TextMesh text;
	// Use this for initialization
	void Start ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_level2Control = GameObject.Find("Manager").GetComponent<Level2Control>();
	}

	private SpriteRenderer _spriteRenderer;
	public EPlayer HeroOnIt { get; private set; } = EPlayer.No;

	// Update is called once per frame
	void Update () {

		/*
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
		*/
		switch (Player)
		{
			case EPlayer.No:
				_spriteRenderer.sprite = NoPlayer;
				break;
			case EPlayer.Player0:
				_spriteRenderer.sprite = BlueSprites[BuildingLevel];
				break;
			case EPlayer.Player1:
				_spriteRenderer.sprite = RedSprites[BuildingLevel];
				break;
		}

		text.text = BuildingLevel.ToString();
	}


	
}

