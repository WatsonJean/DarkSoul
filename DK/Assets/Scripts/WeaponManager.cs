using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public ActorManager am;
    public Collider weaponCollider;
    public GameObject weapon_L;
    public GameObject weapon_R;
    void WeaponEnable(int val)
    {
        weaponCollider.enabled =  val>0;
    }

}
