using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using XLua;

public class TileHero : LuaRunner
{
    private TileHeroData data;
    public Vector2Int pos;
    public Level2Control Level2Control;
    public TileCtrl.EPlayer Player;

    protected override void OStart()
    {
        _tileTick = FindLuaFunc<TurnMethod>("OnTurn");
        data = new TileHeroData(this);
        Level2Control = GameObject.Find("Manager").GetComponent<Level2Control>();
        Level2Control.Players.Add(new Level2Control.TileHeroInfo()
        {
            Player = Player,
            Hero = this
        });
    }

    private float _time = 0;

    protected override void OUpdate()
    {
        // 一秒钟唤醒一次
        _time += Time.deltaTime;
        if (!(_time >= 1.0f)) return;
        _time = 0;
        OnTurn();
    }

    private void OnTurn()
    {
        _tileTick?.Invoke(data);
    }

    #region Delegate

    delegate void TurnMethod(TileHeroData data);

    private TurnMethod _tileTick;

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
                return LookLeft();
            case 1:
                return LookUp();
            case 2:
                return LookRight();
            case 3:
                return LookDown();
            default:
                break;
        }

        return null;
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
}