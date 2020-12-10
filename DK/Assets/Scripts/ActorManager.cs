using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public ActionController ac;
    [Header("=====auto Generate if null======")]
    BattleManager battleMgr;
    WeaponManager weaponMgr;
    AttributeStatusManager attributeMgr;
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
    public void Damage()
    {
        if (attributeMgr.isDenfense)
        {
            Blocked();
        }
        else
        {
            if (attributeMgr.HP > 0)
            {
                if (attributeMgr.AddHP(-1) > 0)
                {
                    Hit();
                }
                else
                    Die();
            }
        }

    }
    public void Blocked()
    {
        ac.IssueTrigger("blocked");
    }
    public void Hit()
    {
        ac.IssueTrigger("hit");
    }

    public void Die()
    {
        ac.IssueTrigger("die");
        ac.mInput.enableInput = false;
        if (ac.cameraController.lockState)
        {
            ac.cameraController.LockUnLock(); 
        }
        ac.cameraController.enabled = false;
    }
}
