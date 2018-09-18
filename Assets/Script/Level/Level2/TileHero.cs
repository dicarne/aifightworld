using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Tilemaps;
using XLua;
using XLua.LuaDLL;

public class TileHero : LuaRunner
{
    private TileHeroData data;
    public Vector2Int pos;
    public Level2Control Level2Control;
    public TileCtrl.EPlayer Player;
    private LuaFunction luaFunction;

    protected override void OStart()
    {
        //_tileTick = FindLuaFunc<TurnMethod>("OnTurn");

        luaFunction = LuaEnv.Global.Get<LuaFunction>("OnTurn");

        data = new TileHeroData(this);
        Level2Control = GameObject.Find("Manager").GetComponent<Level2Control>();
        Level2Control.Players.Add(new Level2Control.TileHeroInfo()
        {
            Player = Player,
            Hero = this
        });
    }

    private float _time = 0;
    private TileCtrl lasttile = null;

    protected override void OUpdate()
    {
        // 一秒钟唤醒一次
        _time += Time.deltaTime;
        if (!(_time >= 0.2f)) return;
        _time = 0;
        //OnTurn();
        if (lasttile) lasttile.Lock = false;
    }

    /// <summary>
    /// 每回合执行
    /// </summary>
    public void OnTurn()
    {
        //_tileTick?.Invoke(data);
        object[] vals = luaFunction.Call(new object[] {data}, new Type[] { });

        if (data.act.op == TileHeroData.Op.Build)
        {
            TileCtrl tile;
            if (Level2Control.Map.TryGetValue(new Vector2Int(data.act.i, data.act.j), out tile))
            {
                tile.Lock = true;
                if (tile.Player == Player || tile.Player == TileCtrl.EPlayer.No)
                {
                    tile.Player = Player;
                    tile.Type = TileCtrl.EType.Building;
                    tile.BuildingLevel += 1;
                }
                else
                {
                    tile.BuildingLevel -= 1;
                    if (tile.BuildingLevel <= 0)
                    {
                        tile.BuildingLevel = 0;
                        tile.Player = TileCtrl.EPlayer.No;
                        tile.Type = TileCtrl.EType.Land;
                    }
                }
            }
        }
    }

    #region Delegate

    [CSharpCallLua]
    delegate void TurnMethod(TileHeroData data);

    [CSharpCallLua] private TurnMethod _tileTick;

    #endregion
}

public class TileHeroData
{
    private TileHero _hero;

    public TileHeroData(TileHero h)
    {
        _hero = h;
    }

    [LuaCallCSharp]
    public TileCtrl Look(int direction)
    {
        switch (direction)
        {
            case 0:
                return LookHere();
            case 1:
                return LookLeft();
            case 2:
                return LookUp();
            case 3:
                return LookRight();
            case 4:
                return LookDown();
            default:
                break;
        }

        return null;
    }

    [LuaCallCSharp]
    public TileCtrl LookHere()
    {
        TileCtrl result;
        var res = _hero.Level2Control.Map.TryGetValue(_hero.pos, out result);
        return res ? result : null;
    }

    [LuaCallCSharp]
    public TileCtrl LookLeft()
    {
        TileCtrl result;
        var res = _hero.Level2Control.Map.TryGetValue(_hero.pos + Vector2Int.left, out result);
        return res ? result : null;
    }

    [LuaCallCSharp]
    public TileCtrl LookRight()
    {
        TileCtrl result;
        var res = _hero.Level2Control.Map.TryGetValue(_hero.pos + Vector2Int.right, out result);
        return res ? result : null;
    }

    [LuaCallCSharp]
    public TileCtrl LookUp()
    {
        TileCtrl result;
        var res = _hero.Level2Control.Map.TryGetValue(_hero.pos + Vector2Int.up, out result);
        return res ? result : null;
    }

    [LuaCallCSharp]
    public TileCtrl LookDown()
    {
        TileCtrl result;
        var res = _hero.Level2Control.Map.TryGetValue(_hero.pos + Vector2Int.down, out result);
        return res ? result : null;
    }

    // TODO: 一回合只能行动一次
    public bool Move(int direction)
    {
        switch (direction)
        {
            case 0:
                return MoveLeft();
            case 1:
                return MoveUp();
            case 2:
                return MoveRight();
            case 3:
                return MoveDown();
            default:
                break;
        }

        return false;
    }

    /// <summary>
    /// 是否能移动到该格子
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    [LuaCallCSharp]
    public bool CanMove(int i, int j)
    {
        int count = 0;
        TileCtrl tile;
        if (!_hero.Level2Control.Map.TryGetValue(new Vector2Int(i, j), out tile))
        {
            return false;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i - 1, j - 1), out tile))
        {
            if (tile.Player == _hero.Player) count++;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i - 1, j), out tile))
        {
            if (tile.Player == _hero.Player) count++;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i - 1, j + 1), out tile))
        {
            if (tile.Player == _hero.Player) count++;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i, j - 1), out tile))
        {
            if (tile.Player == _hero.Player) count++;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i, j + 1), out tile))
        {
            if (tile.Player == _hero.Player) count++;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i + 1, j - 1), out tile))
        {
            if (tile.Player == _hero.Player) count++;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i + 1, j), out tile))
        {
            if (tile.Player == _hero.Player) count++;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i + 1, j + 1), out tile))
        {
            if (tile.Player == _hero.Player) count++;
        }

        if (count >= 1) return true;
        else return false;
    }

    [LuaCallCSharp]
    public bool MoveLeft()
    {
        var tile = LookLeft();
        if (tile == null || tile.HeroOnIt != TileCtrl.EPlayer.No) return false;
        _hero.pos += Vector2Int.left;
        return true;
    }

    [LuaCallCSharp]
    public bool MoveRight()
    {
        var tile = LookRight();
        if (tile == null || tile.HeroOnIt != TileCtrl.EPlayer.No) return false;
        _hero.pos += Vector2Int.right;
        return true;
    }

    [LuaCallCSharp]
    public bool MoveUp()
    {
        var tile = LookUp();
        if (tile == null || tile.HeroOnIt != TileCtrl.EPlayer.No) return false;
        _hero.pos += Vector2Int.up;
        return true;
    }

    [LuaCallCSharp]
    public bool MoveDown()
    {
        var tile = LookDown();
        if (tile == null || tile.HeroOnIt != TileCtrl.EPlayer.No) return false;
        _hero.pos += Vector2Int.down;
        return true;
    }

    [LuaCallCSharp]
    public bool Build()
    {
        // TODO: 建造一次占领，然后建造一次增加一次建筑等级
        return false;
    }

    // ----------------------------
    [LuaCallCSharp]
    public TileInfo map(int i, int j)
    {
        TileCtrl tile;
        var con = _hero.Level2Control.Map.TryGetValue(new Vector2Int(i, j), out tile);
        if (!con) return new TileInfo() {Player = TileCtrl.EPlayer.No};
        return new TileInfo()
        {
            Player = tile.Player,
            Type = tile.Type,
            BuildingLevel = tile.BuildingLevel
        };
    }

    public enum Op
    {
        Sleep,
        Build
    }

    public struct Act
    {
        public Op op;
        public int i;
        public int j;
    }

    public Act act;

    /// <summary>
    /// 执行行动
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="op"></param>
    [LuaCallCSharp]
    public void Action(int i, int j, int op)
    {
        act = new Act();
        switch (op)
        {
            case 0:
                act.op = Op.Sleep;
                break;
            case 1:
                act.op = Op.Build;
                break;
            default:
                act.op = Op.Sleep;
                break;
        }

        act.i = i;
        act.j = j;
    }

    /// <summary>
    /// 周围有几个空格子
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    [LuaCallCSharp]
    public int EmptyAround(int i, int j)
    {
        int count = 0;
        TileCtrl tile;
        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i - 1, j - 1), out tile))
        {
            if (tile.Player == TileCtrl.EPlayer.No) count++;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i - 1, j), out tile))
        {
            if (tile.Player == TileCtrl.EPlayer.No) count++;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i - 1, j + 1), out tile))
        {
            if (tile.Player == TileCtrl.EPlayer.No) count++;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i, j - 1), out tile))
        {
            if (tile.Player == TileCtrl.EPlayer.No) count++;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i, j + 1), out tile))
        {
            if (tile.Player == TileCtrl.EPlayer.No) count++;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i + 1, j - 1), out tile))
        {
            if (tile.Player == TileCtrl.EPlayer.No) count++;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i + 1, j), out tile))
        {
            if (tile.Player == TileCtrl.EPlayer.No) count++;
        }

        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i + 1, j + 1), out tile))
        {
            if (tile.Player == TileCtrl.EPlayer.No) count++;
        }

        return count;
    }

    /// <summary>
    /// 该格子是否能建造
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    [LuaCallCSharp]
    public bool CanBuild(int i, int j)
    {
        TileCtrl tile;
        if (_hero.Level2Control.Map.TryGetValue(new Vector2Int(i, j), out tile))
        {
            if (tile.Player == _hero.Player && tile.BuildingLevel >= 4)
                return false;
            return true;
        }


        return false;
    }

    [LuaCallCSharp]
    public TileCtrl.EPlayer Player
    {
        get { return _hero.Player; }
    }

    [LuaCallCSharp]
    public int Myscore
    {
        get { return _hero.Level2Control.GetScore(_hero.Player); }
    }
}

public struct TileInfo
{
    public TileCtrl.EType Type;
    public TileCtrl.EPlayer Player;
    public int BuildingLevel;
}