using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    ActionController ac;
    BattleManager bm;
    // Start is called before the first frame update
    void Awake()
    {
        ac = GetComponent<ActionController>();
        Transform SensorTrans = transform.Find("Sensor");
        bm = SensorTrans.GetComponent<BattleManager>();
        if (bm == null)
        {
            bm = SensorTrans.gameObject.AddComponent<BattleManager>();
        }
        bm.am = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DoDamage()
    {
        ac.SetTriggerbyName("hit");
    }
}
