using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionController : MonoBehaviour
{
    ActionController actionController;
    Animator animator;

    void Awake()
    {
        actionController = transform.parent.GetComponent<ActionController>();
        animator = GetComponent<Animator>();
    }
    void OnAnimatorMove()
    {
        actionController.OnUpdateRootMotion(animator.deltaPosition);
    }

}
