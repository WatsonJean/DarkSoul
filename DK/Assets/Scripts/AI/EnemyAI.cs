using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    ActorManager actorManager;
    FSMMachine<ActorManager> fSMMachine = new FSMMachine<ActorManager>();
    public Transform target;
    // Start is called before the first frame update
    void Awake()
    {
        actorManager = GetComponent<ActorManager>();
    }

    void Start()
    {
        fSMMachine.AddState(new IdleState(actorManager, fSMMachine));
        fSMMachine.AddState(new PursueState(actorManager, fSMMachine));
        fSMMachine.AddState(new AttackState(actorManager, fSMMachine));
        fSMMachine.AddState(new SkillAttackState(actorManager, fSMMachine));
        
        fSMMachine.ChangeState("Idle",null);
    }

    // Update is called once per frame
    void Update()
    {
        if (actorManager.attributeMgr.isDie)
            return;
        fSMMachine.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (actorManager.attributeMgr.isDie)
            return;
        fSMMachine.FixedUpdate(Time.fixedDeltaTime);
    }
}
