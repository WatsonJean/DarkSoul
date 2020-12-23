using UnityEngine;
using UnityEngine.Playables;

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
    Vector3 actionPos;
    Transform lookatActor;
    bool moveActionPos = false;
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
        actionPos = transform.position;

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
        if (moveActionPos)
        {
            transform.position = Vector3.Slerp(transform.position, actionPos, 0.12f);
            ac.model.transform.LookAt(lookatActor, Vector3.up);
        }
        if (Vector3.Distance(transform.position, actionPos)<0.2f)
        {
            moveActionPos = false;
        }


    }
    public void DoAction()
    {
        if (interActorMgr.casterEventsList.Count>0)
        {
            if (directorMgr.pd.state == PlayState.Playing)//说明在播放中
                return;
            CasterEvent casterEvent = interActorMgr.casterEventsList[0];
            if (casterEvent.active == false)
                return;
            if (casterEvent.eventName == "Stab_timeline")
            {
                if (casterEvent.actorManager.attributeMgr.isStunned && battleMgr.IsFace2FaceFrontAngle(casterEvent.actorManager.ac.model.transform, ac.model.transform, 45)) 
                {
                    actionPos = casterEvent.actorManager.transform.position + casterEvent.actorManager.transform.TransformVector(casterEvent.offset);
                    moveActionPos = true;
                    // transform.position = actionPos;
                    lookatActor = casterEvent.actorManager.transform;
                    // ac.model.transform.LookAt(casterEvent.actorManager.transform,Vector3.up);
                    casterEvent.actorManager.Die();
                    directorMgr.Play_Stab("Stab_timeline", this, casterEvent.actorManager);
                  
                }
            
            }
            else if (casterEvent.eventName == "openBox")
            {
                if (battleMgr.IsFace2FaceFrontAngle(casterEvent.transform, ac.model.transform, 25)) ;
                {
                    actionPos = casterEvent.itemBase.transform.position + casterEvent.itemBase.transform.TransformVector(casterEvent.offset);
                    moveActionPos = true;
                    // transform.position = actionPos;
                    lookatActor = casterEvent.itemBase.transform;
                   // ac.model.transform.LookAt(casterEvent.itemBase.transform, Vector3.up);
                    casterEvent.itemBase.trigger = this;//触发者
                    //casterEvent.active = false;//箱子只能开一次
                    directorMgr.Play_ItemAction("openBox_timeline", casterEvent.itemBase);
                }
            }

            else if (casterEvent.eventName == "switchGear")
            {
                if (battleMgr.IsFace2FaceFrontAngle(casterEvent.transform, ac.model.transform, 25)) ;
                {
                    actionPos = casterEvent.itemBase.transform.position + casterEvent.itemBase.transform.TransformVector(casterEvent.offset);
                   moveActionPos = true;
                    //transform.position = actionPos;
                    lookatActor = casterEvent.itemBase.transform;
                  //  ac.model.transform.LookAt(casterEvent.itemBase.transform, Vector3.up);
                    casterEvent.itemBase.trigger = this;//触发者
                    //casterEvent.active = false;//只能开一次
                    directorMgr.Play_ItemAction("switchgear_timeline", casterEvent.itemBase);
                }
            }
        }
    }

    //受到伤害 处理
    public void TryDamage(WeaponController controller)
    {
        if (attributeMgr.isImmortal)//无敌
        {
            return;
        }
        //攻击方
        ActorManager attacker = controller.wm.actorManager;
        if (attributeMgr.isCounterBackSuccess)//盾反成功动画状态 只能盾反普攻
        {
            if (attacker.attributeMgr.isNormalAttack && battleMgr.CounterBackSelf(attacker.ac.model.transform))//范围内
                attacker.Stunned();
        }
        else if (attributeMgr.isCounterBackFail)//盾反失败动画状态
        {
           if (attacker.battleMgr.AttackFrontSelf(ac.model.transform))//范围内
            {
                DamageHP(controller, false);
            }      
        }
      
      else  if (attributeMgr.isDenfense && battleMgr.IsFace2FaceFrontAngle(attacker.ac.model.transform, ac.model.transform, 70))//防御
        {
                Blocked();       
        }
       else    
        {
            if (attacker.battleMgr.AttackFrontSelf(transform))
                DamageHP(controller);
        }
    }

    void HitEffect()
    {
        HittedMatEffect sc = ac.model.GetComponent<HittedMatEffect>();
        if (sc == null)
            sc = ac.model.AddComponent<HittedMatEffect>();
        sc.Active();
        sc.SetColor(Color.red);
    }
    void DamageHP(WeaponController controller ,bool showHitAnim = true)
    {
        if (attributeMgr.isImmortal)//无敌
            return;
        HitEffect();
        if (attributeMgr.AddHP(controller.GetATK()) > 0)
        {
            if (showHitAnim )
            {
                Hit(controller.wm.actorManager);
            }
           
        }
        else
            Die();
    }
    public void SetCounterBackEventFlag(bool val)
    {
       attributeMgr.isCounterBackEventFlag = val;
    }
    public void Hit(ActorManager attacker)
    {

        ac.IssueTrigger("hit");
        Vector3 dir = (attacker.ac.model.transform.position - ac.model.transform.position).normalized;
        //判断攻击在自己的前后方
        bool isForward = Vector3.Dot(ac.model.transform.forward, dir) > 0 ? true : false;
        //判断攻击在自己的左右方
        bool isRight = Vector3.Dot(ac.model.transform.right, dir) > 0 ? true : false;

        Vector3 v1 = Vector3.ProjectOnPlane(ac.model.transform.forward, Vector3.up);
        Vector3 v2 = Vector3.ProjectOnPlane(dir, Vector3.up);
        float angle = Vector3.Angle(v1, v2);// 夹角 

        //如果攻击者在自己后方，或者在前方并且攻击角度小于30度，播放前方受击动画
        if (! isForward || isForward && angle < 30)
        {
            ac.SetFloat("hitType", 0);
        }
        else //根据左右方位决定播放动画
        {
            ac.SetFloat("hitType", isRight ? 1 : -1);
        }

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
