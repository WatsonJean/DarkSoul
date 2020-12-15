using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IActorManagerInterface
{

    CapsuleCollider capsuleCol;
    public float attackAngle = 45;
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
            Debug.Log("被接触的物体为" + other.name);
        }
    
    }

    public void EnableCollider(bool val)
    {
        capsuleCol.enabled = val;
    }

    // 攻击前方判断
    public bool IsAttackFront(Transform attacker, Transform receiver)
    {
        Vector3 attackDir = receiver.position - attacker.position;
        return Vector3.Angle(attacker.forward, attackDir.normalized) < attackAngle;
    }


    // 盾反前方判断
    public bool IsCounterBackFront(Transform attacker, Transform receiver)
    {
        //先判断是否面对面，在判断夹角
        Vector3 attackDir = (receiver.position - attacker.position).normalized;
        Vector3 counterDir = (attacker.position-receiver.position).normalized ;
  
        return Vector3.Angle(attacker.forward, attackDir.normalized) < attackAngle;
    }
}
