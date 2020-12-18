﻿using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public ActionController ac;
    [Header("=====auto Generate if null======")]
    public BattleManager battleMgr;
    public WeaponManager weaponMgr;
    public AttributeStatusManager attributeMgr;
    public DirectorManager directorMgr;
    public InterActorManager interActorMgr;
    // Start is called before the first frame update
    void Awake()
    {
        ac = GetComponent<ActionController>();
        Transform sensorTrans = transform.Find("Sensor");
        weaponMgr = Bind<WeaponManager>(ac.model);
        if (sensorTrans!=null)
        {
            battleMgr = Bind<BattleManager>(sensorTrans.gameObject);
            interActorMgr = Bind<InterActorManager>(sensorTrans.gameObject);
        }
        attributeMgr = Bind<AttributeStatusManager>(gameObject);
        directorMgr = Bind<DirectorManager>(gameObject);
        ac.OnActionEvents += DoAction;
    }

    T Bind<T>(GameObject go) where T: IActorManagerInterface
    {
        T temp;
        temp = go.GetComponent<T>();
        if (temp == null)
            temp = go.AddComponent<T>();
        temp.actorManager = this;
        return temp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DoAction()
    {
        if (interActorMgr.casterEventsList.Count>0)
        {
            CasterEvent casterEvent = interActorMgr.casterEventsList[0];
            if (casterEvent.active == false)
                return;
            if (casterEvent.eventName == "Stab_timeline")
            {
                if (battleMgr.IsFace2FaceFrontAngle(casterEvent.actorManager.ac.model.transform, ac.model.transform, 45)) 
                {
                  
                    transform.position = casterEvent.actorManager.transform.position + casterEvent.actorManager.transform.forward*0.5f;
                    ac.model.transform.LookAt(casterEvent.actorManager.transform,Vector3.up);
                    directorMgr.Play_Stab("Stab_timeline", this, casterEvent.actorManager);
                }
            
            }
            else if (casterEvent.eventName == "openBox")
            {
                if (battleMgr.IsFace2FaceFrontAngle(casterEvent.transform, ac.model.transform, 15)) ;
                {
                    transform.position = casterEvent.itemBase.transform.position + casterEvent.itemBase.transform.forward * 1.5f;
                    ac.model.transform.LookAt(casterEvent.itemBase.transform, Vector3.up);
                    casterEvent.itemBase.trigger = this;//触发者
                    //casterEvent.active = false;//箱子只能开一次
                    directorMgr.Play_OpenBox("openBox_timeline", casterEvent.itemBase);
                }
            }
        }
    }

    //受到伤害 处理
    public void TryDamage(WeaponController controller)
    {
        //攻击方
        ActorManager attacker = controller.wm.actorManager;
        if (attributeMgr.isCounterBackSuccess)//盾反成功动画状态
        {
            if (battleMgr.CounterBackSelf(attacker.ac.model.transform))//范围内
                attacker.Stunned();
        }
        else if (attributeMgr.isCounterBackFail)//盾反失败动画状态
        {
           if (attacker.battleMgr.AttackFrontSelf(ac.model.transform))//范围内
            {
                DamageHP(-1, false);
            }
         
        }
     else   if (attributeMgr.isImmortal)//无敌
        {
            return;
        }
      else  if (attributeMgr.isDenfense)//防御
        {
            Blocked();
        }
       else    
        {
            if (attacker.battleMgr.AttackFrontSelf(transform))
                DamageHP(-1);
        }

    }

    void DamageHP(float val,bool showHitAnim = true)
    {
        if (attributeMgr.AddHP(val) > 0)
        {
            if (showHitAnim)
            {
                Hit();
            }
           
        }
        else
            Die();
    }
    public void SetCounterBackEventFlag(bool val)
    {
       attributeMgr.isCounterBackEventFlag = val;
    }
    public void Hit()
    {
        ac.IssueTrigger("hit");
    }
    public void Blocked()
    {
        ac.IssueTrigger("blocked");
    }
    public void Stunned()
    {
        ac.IssueTrigger("stunned");
    }

    public void Die(bool showAnim = true)
    {
        battleMgr.EnableCollider(false);

        ac.IssueTrigger("die");
        ac.mInput.enableInput = false;
        if (ac.cameraController.lockState)
        {
            ac.cameraController.LockUnLock(); 
        }
        ac.cameraController.enabled = false;

    }

    public void LockActorController(bool val)
    {
        ac.SetBool("lock", val);
    }
}
