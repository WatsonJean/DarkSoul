using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    ActionController ac;
    BattleManager bm;
    WeaponManager wm;
    // Start is called before the first frame update
    void Awake()
    {
        ac = GetComponent<ActionController>();
        wm = ac.model.GetComponent<WeaponManager>();
        if (wm == null)
            wm = ac.model.AddComponent<WeaponManager>();
        wm.am = this;

        Transform SensorTrans = transform.Find("Sensor");
        bm = SensorTrans.GetComponent<BattleManager>();
        if (bm == null)
            bm = SensorTrans.gameObject.AddComponent<BattleManager>();
        bm.am = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DoDamage()
    {
        ac.IssueTrigger("hit");
    }
}
