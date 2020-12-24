using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    ActorManager actorManager;
    FSMMachine<ActorManager> fSMMachine = new FSMMachine<ActorManager>();
    public Transform target;
    // Start is called before the first frame update
    public float range = 10;
    public int AI_Level = 1;
    void Awake()
    {
        actorManager = GetComponent<ActorManager>();
    }

    void Start()
    {
        fSMMachine.AddState(new IdleState(actorManager, fSMMachine, "Idle"));
        fSMMachine.AddState(new DeadState(actorManager, fSMMachine, "Dead"));
        
        fSMMachine.AddState(new PursueState(actorManager, fSMMachine, "Pursue"));
        if (AI_Level >0)
        {
            fSMMachine.AddState(new AttackState(actorManager, fSMMachine, "Attack1"));
        }
        if (AI_Level >1)
        {
            fSMMachine.AddState(new AttackState(actorManager, fSMMachine, "Attack2"));
        }
        if (AI_Level >2)
        {
            fSMMachine.AddState(new SkillAttackState(actorManager, fSMMachine, "SkillAttack1"));
        }
        if (AI_Level >3)
        {
            fSMMachine.AddState(new SkillAttackState(actorManager, fSMMachine, "SkillAttack2"));
        }



        fSMMachine.ChangeState("Idle",null);
    }

    // Update is called once per frame
    void Update()
    {
        if (actorManager.attributeMgr.isDie)
        {
            fSMMachine.ChangeState("Dead", null);
        }

        fSMMachine.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (actorManager.attributeMgr.isDie)
        {
            fSMMachine.ChangeState("Dead", null);
        }
        fSMMachine.FixedUpdate(Time.fixedDeltaTime);
    }
}
