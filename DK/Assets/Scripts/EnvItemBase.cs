﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvItemBase : MonoBehaviour
{
    public ActorManager  trigger;//触发者;
    public Rigidbody rigbody;
    public Animator mAnimator;
    public GameObject model;
    public string weaponName;
    void Awake()
    {
        rigbody = GetComponent<Rigidbody>();
        mAnimator = GetComponentInChildren<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Lock(bool val)
    {
        SetBool("lock", val);

    }
    public void IssueTrigger(string name)
    {
        if (mAnimator)
        {
            mAnimator.SetTrigger(name);
        }
     
    }

    public void SetBool(string key, bool val)
    {
        if (mAnimator)
        {
            mAnimator.SetBool(key, val);
        }
    }
}
