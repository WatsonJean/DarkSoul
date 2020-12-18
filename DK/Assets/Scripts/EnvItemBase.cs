using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvItemBase : MonoBehaviour
{
    public ActorManager  trigger;//触发者;
    public Rigidbody rigbody;
    public Animator mAnimator;
    public GameObject model;
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
        mAnimator.SetTrigger(name);
    }

    public void SetBool(string key, bool val)
    {
        mAnimator.SetBool(key, val);
    }
}
