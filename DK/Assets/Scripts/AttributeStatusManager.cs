using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeStatusManager : IActorManagerInterface
{
    public float HPMax = 5;
    public float HP = 5;

    [Header("======StateFlag======")]
    public bool isGround = false;
    public bool isJump = false;
    public bool isJab = false;
    public bool isRoll = false;
    public bool isFall = false;
    public bool isAttack = false;
    public bool isDenfense = false;
    public bool isBlocked = false;
    public bool isHit = false;
    public bool isDie = false;
    public float AddHP(float val)
    {
        HP += val;
        HP = Mathf.Clamp(HP, 0, HPMax);
        return HP;
    }

    // Update is called once per frame
    void Update()
    {
        isGround = actorManager.ac.CheckState("Ground");
        isJump = actorManager.ac.CheckState("jump"); ;
        isJab = actorManager.ac.CheckState("jab"); ;
        isRoll = actorManager.ac.CheckState("roll"); ;
        isFall = actorManager.ac.CheckState("fall"); ;
        isAttack = actorManager.ac.CheckStateByTag("attack_R") || actorManager.ac.CheckStateByTag("attack_L");
        isDenfense = actorManager.ac.CheckState("denfense1h", "denfense"); ;
        isBlocked = actorManager.ac.CheckState("blocked"); ;
        isHit = actorManager.ac.CheckState("hit"); ;
        isDie = actorManager.ac.CheckState("die"); ;
    }
}
