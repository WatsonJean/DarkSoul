﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetfArmFixIK : MonoBehaviour
{
    Animator animator;
    public Vector3 offset;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnAnimatorIK()
    {
        Transform transform= animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        transform.localEulerAngles += offset;
        animator.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(transform.localEulerAngles));
    }
}
