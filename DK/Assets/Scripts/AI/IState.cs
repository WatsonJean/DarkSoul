﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class IState<T> //抽象出的公共行为
{
    public T instance;
    public FSMMachine<T> machine;
    public IState(T instance, FSMMachine<T> machine)
    {
        this.instance = instance;
        this.machine = machine;
    }
    /// <summary>
    /// 状态进入
    /// </summary>
    public abstract void OnEnter(T obj );
    /// <summary>
    /// 状态更新
    /// </summary>
    public abstract void OnUpdate(float time);

    /// <summary>
    /// 状态更新
    /// </summary>
    public abstract void OnFixedUpdate(float time);
    /// <summary>
    /// 状态退出
    /// </summary>
    public abstract void OnExit();
    /// <summary>
    /// 获得状态
    /// </summary>
    public abstract string GetState();
}
