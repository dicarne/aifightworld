using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class Hero : LuaRunner
{
    protected override void BindLocalMethod()
    {
        heroTick = FindLuaFunc<HeroTick>("HeroTick");
    }

    protected override void OStart()
    {
        _heroData = new Hero2Lua();
        _heroData.myX = gameObject.transform.position.x;
        _heroData.myY = gameObject.transform.position.y;
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

        if (!(Math.Abs(diff) >= 1)) return;
        if (diff > 0)
        {
            transform.Rotate(0, 0, 30.0f * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, 0, -30.0f * Time.deltaTime);
        }
    }

    #region Data
    
    /// <summary>
    /// 储存与Lua交互的数据和方法
    /// </summary>
    private Hero2Lua _heroData;
    
    /// <summary>
    /// 移动的速度
    /// </summary>
    public float MoveSpeed { get; private set; } = 10.0f;

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

    [LuaCallCSharp]
    public void MoveToXY(float x, float y)
    {
        aimX = x;
        aimY = y;
    }

    [LuaCallCSharp]
    public void Rotate(float angle)
    {
        this.angle = angle;
    }

    public void StopMove()
    {
        aimX = myX;
        aimY = myY;
    }
}