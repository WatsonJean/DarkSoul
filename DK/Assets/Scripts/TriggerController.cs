﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//动画事件挂载有此类
public class TriggerController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
   public  void ResetTrigger(string paraName)
    {
        animator.ResetTrigger(paraName);
    }
}
