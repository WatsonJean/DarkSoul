using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<ActorManager>
{
    ActorManager target;


    public AttackState(ActorManager ac, FSMMachine<ActorManager> fsm, string name) : base(ac, fsm,name)
    {
        base.instance = ac;

    }

    public override void OnEnter(ActorManager obj )
    {
        if (obj != null)
        {
            target = (ActorManager)obj;
        }
        instance.transform.LookAt(target.transform, Vector3.up);
    


    }

    public override void OnExit()
    {
        instance.ac.mInput.attack1 = false;
        instance.ac.mInput.attack2 = false;
    }


    public override void OnFixedUpdate(float time)
    {

    }

    public override void OnUpdate(float time)
    {
        if (target == null || target.attributeMgr.isDie || instance.attributeMgr.isHit || instance.ac.CheckState("attack1h_D") || instance.ac.CheckState("attack2_C") )
        {
            machine.ChangeState("Idle",null);
            return;
        }

        instance.transform.LookAt(target.transform, Vector3.up);
        instance.ac.mInput.attack1 = base.stateName == "Attack1";
        instance.ac.mInput.attack2 = base.stateName == "Attack2";
    }

}
