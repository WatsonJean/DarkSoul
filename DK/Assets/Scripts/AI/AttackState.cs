using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<ActorManager>
{
    ActorManager target;


    public AttackState(ActorManager ac, FSMMachine<ActorManager> fsm) : base(ac, fsm)
    {
        base.instance = ac;
        base.machine = fsm;
    }

    public override string GetState()
    {
        return "Attack";
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

    }


    public override void OnFixedUpdate(float time)
    {

    }

    public override void OnUpdate(float time)
    {
        if (target == null || target.attributeMgr.isDie || instance.attributeMgr.isHit || instance.ac.CheckState("attack1h_D"))
        {
            machine.ChangeState("Idle",null);
            return;
        }

        //if (Vector3.Distance(target.transform.position, instance.transform.position) >= 2.5)//距离远了就切换追击
        //{
        //    machine.ChangeState("Pursue", target);
        //    return;
        //}
        instance.transform.LookAt(target.transform, Vector3.up);
        instance.ac.mInput.attack1 = true;
    }
}
