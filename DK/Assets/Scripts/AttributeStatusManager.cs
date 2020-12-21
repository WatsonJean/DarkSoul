using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeStatusManager : IActorManagerInterface
{
    public float HPMax = 100;
    public float HP = 100;
    public float ATK = 1;
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
    public bool isCounterBackState = false; //盾反动画中
    public bool isCounterBackEventFlag = false; //盾反事件开启盾反效果
    public bool isCounterBackSuccess = false; //盾反成功
    public bool isCounterBackFail = false; //盾反失败
    public bool isImmortal = false;//无敌

    private void Awake()
    {
        HP = HPMax;
    }
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
        isFall = actorManager.ac.CheckStateByTag("fall"); ;
        isAttack = actorManager.ac.CheckStateByTag("attack_R") || actorManager.ac.CheckStateByTag("attack_L");
        isDenfense = actorManager.ac.CheckState("denfense1h", "denfense"); ;
        isBlocked = actorManager.ac.CheckState("blocked"); ;
        isHit = actorManager.ac.CheckState("hit"); ;
        isDie = actorManager.ac.CheckState("die");
        isImmortal = isRoll || isJab;
        isCounterBackState = actorManager.ac.CheckState("counterBack");
        isCounterBackSuccess = isCounterBackEventFlag && isCounterBackState;
        isCounterBackFail = ! isCounterBackEventFlag && isCounterBackState;

    }

}
