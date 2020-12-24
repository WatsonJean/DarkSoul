using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface
{
    public Transform weaponHandle_L;
    public Transform weaponHandle_R;
    public Collider  weaponCollider_L;
    public Collider  weaponCollider_R;
    public WeaponController weaponController_L;
    public WeaponController  weaponController_R;

    public string left_weapon = "dun1";
    public string right_weapon = "dao1";
    void Awake()
    {
        weaponHandle_L = transform.DeepFind("weaponHandle_L");
        weaponHandle_R = transform.DeepFind("weaponHandle_R");
        weaponController_L = BindWeaponController(weaponHandle_L.gameObject);
        weaponController_R = BindWeaponController(weaponHandle_R.gameObject);

    }

    private void Start()
    {
        EquipWeapon_L(left_weapon, weaponController_L.weaponPos);
        EquipWeapon_R(right_weapon, weaponController_R.weaponPos);
        weaponCollider_L.enabled = false;
        weaponCollider_R.enabled = false;
    }

    WeaponController BindWeaponController(GameObject go)
    {
        WeaponController temp = go.GetComponent<WeaponController>();
        if (temp==null)
        {
            temp = go.AddComponent<WeaponController>();
        }
        temp.wm = this;
        return temp;
    }

    //攻击动画中挂载的事件
    public void WeaponEnable(int val)
    {
        if (!left_weapon.Contains("dun"))
        {
            weaponCollider_L.enabled = (actorManager.ac.CheckStateByTag("attack") || actorManager.ac.CheckStateByTag("skill")) && val > 0;
        }
        else
            weaponCollider_L.enabled = false;


        weaponCollider_R.enabled = (actorManager.ac.CheckStateByTag("attack")  || actorManager.ac.CheckStateByTag("skill") )&&  val >0;
        //weaponCollider_L.enabled = false;
        //weaponCollider_R.enabled = false;
    }

    //盾反动画中挂载的事件
    public void SetCounterBack(int  val)
    {
        bool b = val > 0 ? true : false;
        actorManager.SetCounterBackEventFlag(b);
    }

    public void EquipWeapon_L(string name,Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
        GameObject go = GameManager.instance.weaponFactory.CreateWeapon(name, parent,Vector3.zero,Quaternion.identity);
        weaponCollider_L = go.GetComponentInChildren<Collider>();
        left_weapon = name;
    }

    public void EquipWeapon_R(string name, Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
        GameObject go = GameManager.instance.weaponFactory.CreateWeapon(name, parent, Vector3.zero, Quaternion.identity);
        weaponCollider_R = go.GetComponentInChildren<Collider>();
        right_weapon = name;
    }
    public void ResetTrigger(string paraName)
    {
     actorManager.ac.mAnimator.ResetTrigger(paraName);
    }

    public void EnableEffect(string name)
    {
        actorManager.effectMgr.SwitchEffect(name, true);
    }
    public void DisableEffect(string name)
    {
        actorManager.effectMgr.SwitchEffect(name, false);
    }
    public void InstantiateEffect(string name)
    {
       actorManager.effectMgr.InstantiateEffect(name, actorManager.transform.position, actorManager.transform.rotation);
    }
}
