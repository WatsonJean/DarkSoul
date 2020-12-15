using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public ActionController ac;
    [Header("=====auto Generate if null======")]
    public BattleManager battleMgr;
    public WeaponManager weaponMgr;
    public AttributeStatusManager attributeMgr;
    // Start is called before the first frame update
    void Awake()
    {
        ac = GetComponent<ActionController>();
        Transform sensorTrans = transform.Find("Sensor");
        weaponMgr = Bind<WeaponManager>(ac.model);   
        battleMgr = Bind<BattleManager>(sensorTrans.gameObject);
        attributeMgr = Bind<AttributeStatusManager>(gameObject);
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
    public void TryDamage(WeaponController controller)
    {
        
        if (attributeMgr.isCounterBackSuccess)//盾反成功
        {
            controller.wm.actorManager.Stunned();
        }
        else if (attributeMgr.isCounterBackFail)//盾反失败
        {
            DamageHP(-1,false);
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

    public void Die()
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
}
