using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueState : IState<ActorManager>
{
    ActorManager target;
    float attackNum = 0;
    public PursueState(ActorManager ac, FSMMachine<ActorManager> fsm,string name) : base(ac, fsm, name)
    {
        base.instance = ac;
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
        instance.ac.mInput.run = false;
        instance.ac.mInput.inputY = 0;
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
        Vector3 vec1 = Vector3.Project(target.transform.position - instance.transform.position, Vector3.forward);
        float dis = Vector3.Distance(target.transform.position, instance.transform.position);

        dis = vec1.magnitude;

        if (dis >20f)
        {
            machine.ChangeState("Idle", null);
        }
      else  if (dis >= 2.5f && dis <=20f)
        {
            instance.transform.LookAt(target.transform, Vector3.up);
            instance.ac.mInput.run = true;
            instance.ac.mInput.inputY = 1;
        }
        else
        {
            instance.ac.mInput.run = false;
            instance.ac.mInput.inputY =0;
            attackNum = Random.value * 100;

            if (attackNum < 25)
                machine.ChangeState("Attack1", target);
            else if (attackNum >= 25 && attackNum<50)
                machine.ChangeState("Attack2", target);
            else if (attackNum >= 50 && attackNum < 75)
                machine.ChangeState("SkillAttack1", target);
            else
                machine.ChangeState("SkillAttack2", target);


            Debug.Log("attackNum: "+ attackNum);
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
