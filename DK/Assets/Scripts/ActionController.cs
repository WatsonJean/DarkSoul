using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public float runSpeed = 5;
    public float walkSpeed = 1.4f;
    public float JumpVelocity = 3f;
    public float rollVelocity = 1f;
    public float rollthreshold = 4;//判断是否能翻滚的阈值
    public GameObject model;
    [Space(10)] 
    [Header("======  PhysicMaterial Setting ======")]
    public PhysicMaterial phy_Mat_zero;
    public PhysicMaterial phy_Mat_one;
    Animator mAnimator;
    PlayerInput mPlayerInput;
    Rigidbody rigidbody;
    CapsuleCollider capsuleCollider;
    Vector3 planeMoveVec;
    Vector3 thrustVelocity=Vector3.zero;//跳跃向前的速度
    float targetSpeed;
    float lerpTarget;
    bool lockPlaneVec = false;
    bool isGround = false;
    bool canAttackFlag = false;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        mPlayerInput = GetComponent<PlayerInput>();
        mAnimator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
    }

    void Start()
    {
    }

    void Update()
    {
        targetSpeed = mPlayerInput.Runing ()? runSpeed : walkSpeed;
        if (mPlayerInput.Dmag > 0.1)
               model.transform.forward =Vector3.Slerp(model.transform.forward, mPlayerInput.CurrVec, 0.3f) ;


        if (mAnimator)
        {
            float target = mPlayerInput.Dmag * targetSpeed;
            float val = Mathf.Lerp(mAnimator.GetFloat("forwordSpeed"), target,0.1f);
            mAnimator.SetFloat("forwordSpeed", val);
            if (mPlayerInput.Jump())
            {
                canAttackFlag = false;
                mAnimator.SetTrigger("jump");         
            }
            if (mPlayerInput.Attack() && CanAttack())
            {
                mAnimator.SetTrigger("attack");
            }
            if (rigidbody.velocity.magnitude > rollthreshold)
                mAnimator.SetTrigger("roll");
        }

        if (!lockPlaneVec)
            planeMoveVec = model.transform.forward * mPlayerInput.Dmag * targetSpeed;

    }

    void FixedUpdate()
    {
        rigidbody.velocity = new Vector3(planeMoveVec.x, rigidbody.velocity.y, planeMoveVec.z) + thrustVelocity;
        thrustVelocity = Vector3.zero;

    }

    bool CheckState(string name,string layername = "Base Layer")
    {
        int layerIndex = mAnimator.GetLayerIndex(layername); 
        return mAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsName(name);
    }

    bool CanAttack()
    {
        return CheckState("Ground") && canAttackFlag;
    }

    void OnJumpEnter()
    { 
        thrustVelocity = new Vector3(0, JumpVelocity, 0);
        mPlayerInput.enableInput = false;
         lockPlaneVec = true;
    }


   public void IsGround()
    {
        isGround = true;
        mAnimator.SetBool("isGround", isGround);
    }

    public void IsNotGround()
    {
        isGround = false;
        mAnimator.SetBool("isGround", isGround);

    }

    public void OnFallEnter()
    {
        mPlayerInput.enableInput = false;
        lockPlaneVec = true;
    }
    public void OnGroundEnter()
    {
        mPlayerInput.enableInput = true;
        lockPlaneVec = false;
        canAttackFlag = true;
        capsuleCollider.material = phy_Mat_one;
    }

    public void OnGroundExit()
    {
        capsuleCollider.material = phy_Mat_zero;
    }

    public void OnRollEnter()
    {
        thrustVelocity =  new Vector3(0, rollVelocity, 0);
        mPlayerInput.enableInput = false;
        lockPlaneVec = true;
    } 
     public void OnJabEnter()
    {
        mPlayerInput.enableInput = false;
        lockPlaneVec = true;
    }
    public void OnJabUpdate()
    {
     
        thrustVelocity = model.transform.forward * mAnimator.GetFloat("jabVelocity");
    }

    public void OnAttack1hA_Enter()
    {
      
        mPlayerInput.enableInput = false;
        lerpTarget = 1;
    }

    public void OnAttack_IdleEnter()
    {
      
        mPlayerInput.enableInput = true;
        lerpTarget = 0;
    }
    public void OnAttack_IdleUpdate()
    {
        thrustVelocity = model.transform.forward * mAnimator.GetFloat("attack1hAVelocity");
        //插值权重
        int layerIndex = mAnimator.GetLayerIndex("attack");
        float currWeight = mAnimator.GetLayerWeight(layerIndex);
        float currLerp = Mathf.Lerp(currWeight, lerpTarget,0.05f);
        mAnimator.SetLayerWeight(layerIndex, currLerp);
    }
    public void OnAttack1hA_Update()
    {

        thrustVelocity = model.transform.forward * mAnimator.GetFloat("attack1hAVelocity");
        //插值权重
        int layerIndex = mAnimator.GetLayerIndex("attack");
        float currWeight = mAnimator.GetLayerWeight(layerIndex);
        float currLerp = Mathf.Lerp(currWeight, lerpTarget, 0.2f);
        mAnimator.SetLayerWeight(layerIndex, currLerp);
    }
    
}
