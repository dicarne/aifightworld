using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XLua;

public class LuaRunner : MonoBehaviour
{
    protected LuaEnv LuaEnv;


    
    // Use this for initialization
    protected void Start()
    {

        LuaEnv = new LuaEnv();
        BindEvent += Binding;
        BindEvent += BindLocalMethod;

        UUpdate += OUpdate;
        UStart += OStart;
        
        BindEvent?.Invoke();
        UStart?.Invoke();
    }


    protected T FindLuaFunc<T>(string name)
    {
        return LuaEnv.Global.Get<T>(name);
    }

    void Binding()
    {
        MTick = FindLuaFunc<Tick>("Tick");
    }

    // Update is called once per frame
    void Update()
    {
        MTick?.Invoke();
        UUpdate?.Invoke();
    }

    protected string Code = "";

    public void ChangeCode(string newCode)
    {
        Code = newCode;
        LuaEnv.DoString(Code);
        
        BindEvent?.Invoke();
        UStart?.Invoke();
    }

    protected delegate void Tick();

    protected Tick MTick;

    protected event Tick BindEvent;
    protected event Tick UUpdate;
    protected event Tick UStart;
    
    #region Virtual Method
    /// <summary>
    /// 用于绑定派生对象独特的方法
    /// </summary>
    protected virtual void BindLocalMethod()
    {
    }
    
    /// <summary>
    /// 用于替代派生对象的Update
    /// </summary>
    protected virtual void OUpdate()
    {
    }

    /// <summary>
    /// 用于替代派生对象的Start
    /// </summary>
    protected virtual void OStart()
    {
    }

    #endregion
}