using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState<ActorManager>
{
    public float tempTime = 0;
    public float intervalTime = 1f;
    public IdleState(ActorManager ac, FSMMachine<ActorManager> fsm) : base(ac, fsm)
    {
        base.instance = ac;
        base.machine = fsm;
    }

    public override string GetState()
    {
        return "Idle";
    }

    public override void OnEnter(ActorManager obj )
    {
        instance.ac.mInput.enableInput = false;
        tempTime = -1;
    }

    public override void OnExit()
    {
        instance.ac.mInput.enableInput = true;
    }

    public override void OnUpdate(float time)
    {

    }

    public override void OnFixedUpdate(float time)
    {
        tempTime += time;
        if (tempTime >= intervalTime)
        {
            tempTime = 0;
            Look();  
        }

    }

    void Look()
    {
        int layerMask = 1 << LayerMask.NameToLayer("player");
        Collider[] colliders = Physics.OverlapSphere(instance.transform.position, 15f, layerMask);

        if (colliders.Length <= 0)
            return;
        else
        {
            base.machine.ChangeState("Pursue", colliders[0].gameObject.GetComponent<ActorManager>());
            Debug.Log(colliders[0].name);
        }
    }
    
}
