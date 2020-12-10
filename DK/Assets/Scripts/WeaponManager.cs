using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface
{
    Transform weaponHandle_L;
    Transform weaponHandle_R;
    Collider weaponCollider_L;
    Collider weaponCollider_R;
    void Awake()
    {
        weaponHandle_L = transform.DeepFind("weaponHandle_L");
        weaponHandle_R = transform.DeepFind("weaponHandle_R");
        weaponCollider_L = weaponHandle_L.GetComponentInChildren<Collider>();
        weaponCollider_R = weaponHandle_R.GetComponentInChildren<Collider>();
        weaponCollider_L.enabled = false;
        weaponCollider_R.enabled = false;
    }
    public void WeaponEnable(int val)
    {
        weaponCollider_L.enabled = actorManager.ac.CheckStateByTag("attack_L") && val > 0;
        weaponCollider_R.enabled = actorManager.ac.CheckStateByTag("attack_R") &&  val >0;
    }

}
