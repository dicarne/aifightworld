﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    void Start()
    {
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
    }

    public void StartTurn()
    {
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
    void Update()
    {
    }

    /// <summary>
    /// 获取某阵营的分数
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
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
    public int MaxTurn = 100;

    /// <summary>
    /// 0.2秒执行一回合
    /// </summary>
    /// <returns></returns>
    IEnumerator Turn()
    {
        var AnotherGameObj = GameObject.Find("Player1").GetComponent<TileHero>();
        var TargetGameObj = GameObject.Find("Player0").GetComponent<TileHero>();
        AnotherGameObj.ChangeCode("require 'script'");

        while (turnnum < MaxTurn)
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
                if (tl.Value.Player != TileCtrl.EPlayer.No) continue;
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
                }
                else if (countB == 4)
                {
                    tl.Value.Player = TileCtrl.EPlayer.Player1;
                    tl.Value.BuildingLevel = 3;
                    tl.Value.Type = TileCtrl.EType.Building;
                }
            }
        }

        if (GetScore(TargetGameObj.Player) > GetScore(AnotherGameObj.Player))
        {
            WinPanel.SetActive(true);
        }
        else
        {
            FailedPanel.SetActive(true);
        }
    }

    public GameObject FailedPanel;
    public GameObject WinPanel;

    /// <summary>
    /// 重置场景
    /// </summary>
    public void Reset()
    {
        FailedPanel.SetActive(false);
        WinPanel.SetActive(false);
        StopAllCoroutines();
        foreach (var kv in Map)
        {
            kv.Value.Player = TileCtrl.EPlayer.No;
            kv.Value.BuildingLevel = 0;
            kv.Value.Type = TileCtrl.EType.Land;
        }

        Map[new Vector2Int(0, 0)].Player = TileCtrl.EPlayer.Player0;
        Map[new Vector2Int(0, 0)].Type = TileCtrl.EType.Building;
        Map[new Vector2Int(9, 9)].Player = TileCtrl.EPlayer.Player1;
        Map[new Vector2Int(9, 9)].Type = TileCtrl.EType.Building;
        turnnum = 0;
    }

    public GameObject HelpPanel;

    public void OpenHelpPanel()
    {
        HelpPanel.SetActive(true);
    }

    public void CloseHelpPanel()
    {
        HelpPanel.SetActive((false));
    }
}