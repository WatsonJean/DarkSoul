using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IState<ActorManager>
{
    public float tempTime = 0;
    public float intervalTime = 1f;
    public DeadState(ActorManager ac, FSMMachine<ActorManager> fsm,string name) : base(ac, fsm, name)
    {
        base.instance = ac;
    }


    public override void OnEnter(ActorManager obj )
    {
        instance.ac.mInput.enableInput = false;

    }

    public override void OnExit()
    {

    }

    public override void OnUpdate(float time)
    {

    }

    public override void OnFixedUpdate(float time)
    {


    }


    
}
