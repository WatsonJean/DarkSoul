using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueState : IState<ActorManager>
{
    ActorManager target;
    float attackNum = 0;
    public PursueState(ActorManager ac, FSMMachine<ActorManager> fsm) : base(ac, fsm)
    {
        base.instance = ac;
        base.machine = fsm;
    }

    public override string GetState()
    {
        return "Pursue";
    }

    public override void OnEnter(ActorManager obj )
    {
        if (obj != null)
        {
            target = (ActorManager)obj;
        }
        attackNum = Random.value * 100;
    }

    public override void OnExit()
    {

    }

    public override void OnFixedUpdate(float time)
    {

    }

    public override void OnUpdate(float time)
    {
        if (target == null || target.attributeMgr.isDie)
        {
            machine.ChangeState("Idle",null);
            return;
        }

        float dis = Vector3.Distance(target.transform.position, instance.transform.position);
        if (dis >15f)
        {
            machine.ChangeState("Idle", null);
        }
      else  if (dis >= 2.5f && dis <=15f)
        {
            instance.transform.LookAt(target.transform, Vector3.up);
            instance.ac.mInput.run = true;
            instance.ac.mInput.inputY = 1;
        }
        else
        {
            instance.ac.mInput.run = false;
            instance.ac.mInput.inputY =0;
            machine.ChangeState("SkillAttack", target);
            Debug.Log("进行攻击状态！");
        }
    }
    void PlayAttack()
    {
        instance.ac.mInput.attack1 = attackNum < 20;
        instance.ac.mInput.attack2 = attackNum >= 20 && attackNum < 40;
        instance.ac.mInput.skill1 = attackNum >= 40 && attackNum < 60;
        instance.ac.mInput.skill2 = attackNum >= 60 && attackNum < 80;
        instance.ac.mInput.skill3 = attackNum >= 80 && attackNum < 100;
    }

}
