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
    void Awake()
    {
        weaponHandle_L = transform.DeepFind("weaponHandle_L");
        weaponHandle_R = transform.DeepFind("weaponHandle_R");
        weaponCollider_L = weaponHandle_L.GetComponentInChildren<Collider>();
        weaponCollider_R = weaponHandle_R.GetComponentInChildren<Collider>();
        weaponCollider_L.enabled = false;
        weaponCollider_R.enabled = false;
        weaponController_L = BindWeaponController(weaponHandle_L.gameObject);
        weaponController_R = BindWeaponController(weaponHandle_R.gameObject);
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
        weaponCollider_L.enabled = actorManager.ac.CheckStateByTag("attack_L") && val > 0;
        weaponCollider_R.enabled = actorManager.ac.CheckStateByTag("attack_R") &&  val >0;
    }

    //盾反动画中挂载的事件
    public void SetCounterBack(int  val)
    {
        bool b = val > 0 ? true : false;
        actorManager.SetCounterBackEventFlag(b);
    }

}
