﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XLua;

public class Hero : LuaRunner
{
    private GameObject HitTarget;
    private GameObject HeroHitEnd;
    protected override void BindLocalMethod()
    {
        heroTick = FindLuaFunc<HeroTick>("HeroTick");
    }

    protected override void OStart()
    {
        HitTarget = gameObject.transform.Find("HeroHitTarget").gameObject;
        HeroHitEnd = gameObject.transform.Find("HeroHitEnd").gameObject;
        _heroData = new Hero2Lua(this)
        {
            myX = gameObject.transform.position.x,
            myY = gameObject.transform.position.y
        };
        _heroData.StopMove();
    }

    protected override void OUpdate()
    {
        _heroData.myX = gameObject.transform.position.x;
        _heroData.myY = gameObject.transform.position.y;

        // Lua设定目标阶段
        heroTick?.Invoke(_heroData);

        // C#执行阶段
        MoveToXy();
        Rotate();
    }

    /// <summary>
    /// 将自身向目标移动
    /// </summary>
    private void MoveToXy()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(_heroData.aimX, _heroData.aimY, 0.0f),
            Time.deltaTime * MoveSpeed);
    }

    /// <summary>
    /// 旋转自身面向，向右为0°
    /// </summary>
    private void Rotate()
    {
         
        var diff = _heroData.angle - transform.rotation.eulerAngles.z;
        if (!(Math.Abs(diff) >= 5)) return;
        // TODO: 旋转方向不对
        if (diff > 0)
        {
            transform.Rotate(0, 0, 60.0f * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, 0, -60.0f * Time.deltaTime);
        }
    }

    public List<LookInfo> Witness()
    {
        RaycastHit2D[] hitResult = new RaycastHit2D[10];
        Physics2D.LinecastNonAlloc(new Vector2(HitTarget.transform.position.x, HitTarget.transform.position.y),
            new Vector2(HeroHitEnd.transform.position.x, HeroHitEnd.transform.position.y), hitResult,
            1 << LayerMask.NameToLayer("GameModel"));

        return (from hitobj in hitResult
            where hitobj.transform != null
            select new LookInfo()
            {
                position = new Vector2(hitobj.transform.position.x, hitobj.transform.position.y),
                angle = hitobj.transform.eulerAngles.z,
                Name = hitobj.transform.gameObject.GetComponent<IDinfo>().Name,
                Type = hitobj.transform.gameObject.GetComponent<IDinfo>().Type,
                point = hitobj.point,
                Distance = hitobj.distance
            }).ToList();
    }

    #region Data

    /// <summary>
    /// 储存与Lua交互的数据和方法
    /// </summary>
    private Hero2Lua _heroData;

    /// <summary>
    /// 移动的速度
    /// </summary>
    public float MoveSpeed { get; private set; } = 20.0f;

    #endregion


    #region Delegate

    [CSharpCallLua]
    delegate void HeroTick(Hero2Lua self);

    private HeroTick heroTick;

    #endregion
}

public class Hero2Lua
{
    public float myX, myY;
    public float aimX;
    public float aimY;
    public float angle;
    private readonly Hero _hero;

    public Hero2Lua(Hero h)
    {
        _hero = h;
    }

    [LuaCallCSharp]
    public void MoveToXY(float x, float y)
    {
        aimX = x;
        aimY = y;
    }

    [LuaCallCSharp]
    public void Rotate(float angle)
    {
        if (angle < 0) angle += 360;
        this.angle = angle;
    }

    [LuaCallCSharp]
    public void StopMove()
    {
        aimX = myX;
        aimY = myY;
    }
    
    [LuaCallCSharp]
    public List<LookInfo> Look()
    {
        return _hero.Witness();
    }
}