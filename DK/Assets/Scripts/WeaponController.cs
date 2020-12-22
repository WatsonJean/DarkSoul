using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponManager wm;
    public Transform weaponPos;
    public WeaponData weaponData;
    void Awake()
    {
        weaponPos = transform.Find("weaponPos");
      
       
    }

    private void Start()
    {
        weaponData = GetComponentInChildren<WeaponData>();
    }

    // Update is called once per frame
    public float GetATK()
    {
        return wm.actorManager.attributeMgr.ATK + weaponData.data.ATK;
    }
}
