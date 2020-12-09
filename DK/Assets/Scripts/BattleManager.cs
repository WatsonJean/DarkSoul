using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : MonoBehaviour
{
    public ActorManager am;
    CapsuleCollider capsuleCol;
    void Start()
    {
        capsuleCol = GetComponent<CapsuleCollider>();
        capsuleCol.center = Vector3.up;
        capsuleCol.height = 2f;
        capsuleCol.radius = 0.25f;
        capsuleCol.isTrigger = true;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag =="weapon")
        {
            am.DoDamage();
            Debug.Log("被接触的物体为" + other.name);
        }
     

    }
}
