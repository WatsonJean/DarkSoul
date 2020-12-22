using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IActorManagerInterface
{
    CapsuleCollider capsuleCol;

    void Start()
    {
        capsuleCol = GetComponent<CapsuleCollider>();
        capsuleCol.center = Vector3.up;
        capsuleCol.height = 2f;
        capsuleCol.radius = 0.25f;
        capsuleCol.isTrigger = true;
        capsuleCol.enabled = true;
    }


    void OnTriggerEnter(Collider other)
    {
       
        if (other.tag =="weapon")
        {
            WeaponController wc = other.GetComponentInParent<WeaponController>();
            actorManager.TryDamage(wc);
            Debug.Log("攻击我的的物体为 " + other.name);
        }
    
    }

    public void EnableCollider(bool val)
    {
        capsuleCol.enabled = val;
    }

    public bool AttackFrontSelf(Transform receiver, float attackAngle = 70)
    {

        return CheckFrontAngle(actorManager.transform, receiver, attackAngle);
    }

    // 攻击前方范围判断
    public bool CheckFrontAngle(Transform attacker, Transform receiver, float attackAngle = 70)
    {
        Vector3 attackDir = receiver.position - attacker.position;
        return Vector3.Angle(attacker.forward, attackDir.normalized) <= attackAngle;
    }

    public bool CounterBackSelf(Transform attacker, float angle = 45)
    {
        return IsFace2FaceFrontAngle(attacker, actorManager.transform, angle);
    }

    // 盾反前方判断
    public bool IsFace2FaceFrontAngle(Transform attacker, Transform counter, float angle = 45)
    {
        //先判断是否面对面，在判断夹角
        Vector3 counterDir = (attacker.position-counter.position).normalized ;
        float counterAngle1 = Vector3.Angle(counter.forward, counterDir);
        float counterAngle2 = Vector3.Angle(attacker.forward, counter.forward);
        //在盾反范围内 && 面对面
        bool inRange = counterAngle1 <= angle && Mathf.Abs(counterAngle2-180)  <= angle;
        return inRange;
    }
}
