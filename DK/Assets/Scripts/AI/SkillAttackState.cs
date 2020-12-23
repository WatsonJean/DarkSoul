using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackState : IState<ActorManager>
{
    ActorManager target;
    bool endFlag = false;

    public SkillAttackState(ActorManager ac, FSMMachine<ActorManager> fsm) : base(ac, fsm)
    {
        base.instance = ac;
        base.machine = fsm;
    }

    public override string GetState()
    {
        return "SkillAttack";
    }

    public override void OnEnter(ActorManager obj )
    {
        if (obj != null)
        {
            target = (ActorManager)obj;
        }
        instance.transform.LookAt(target.transform, Vector3.up);
        endFlag = false;


    }

    public override void OnExit()
    {
        instance.ac.mInput.skill2 = false;

    }


    public override void OnFixedUpdate(float time)
    {

    }

    public override void OnUpdate(float time)
    {
        if (target == null || target.attributeMgr.isDie || instance.attributeMgr.isHit)
        {
            machine.ChangeState("Idle",null);
            return;
        }
        instance.transform.LookAt(target.transform, Vector3.up);
        instance.ac.mInput.skill2 = true;
        if (instance.attributeMgr.isSkillAttack)
        {
            endFlag = true;
            instance.ac.mInput.skill2 = false;
        }
        if (endFlag && instance.attributeMgr.isGround)
        {
           
            machine.ChangeState("Idle", null);
            return;
        }

    }
}
