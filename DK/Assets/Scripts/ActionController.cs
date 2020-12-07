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
    public CameraController cameraController;
    [Space(10)] 
    [Header("======  PhysicMaterial Setting ======")]
    public PhysicMaterial phy_Mat_zero;
    public PhysicMaterial phy_Mat_one;
    Animator mAnimator;
    PlayerInput mPlayerInput;
    Rigidbody rigbody;
    CapsuleCollider capsuleCollider;
    Vector3 planeMoveVec;
    Vector3 thrustVelocity=Vector3.zero;//跳跃向前的速度
    Vector3 deltaPos_Rm; //rootmotion中动画的偏移量

    float lerpTarget;
    bool lockPlaneVec = false;
    bool isGround = false;
    bool canAttackFlag = true;


    void Awake()
    {
        rigbody = GetComponent<Rigidbody>();
        mPlayerInput = GetComponent<PlayerInput>();
        mAnimator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
    }

    void Start()
    {
    }

    void Update()
    {

        float moveSpeed = mPlayerInput.run ? runSpeed : walkSpeed;
        if (mPlayerInput.lockTarget)
        {
            cameraController.LockUnLock();
        }
        if ( ! cameraController.lockState)
        {
            if (mPlayerInput.Dmag > 0.1)
                model.transform.forward = Vector3.Slerp(model.transform.forward, mPlayerInput.CurrVec, 0.3f);
            if (!lockPlaneVec)
                planeMoveVec = model.transform.forward * mPlayerInput.Dmag * moveSpeed;
        }
        else
        {
            model.transform.forward =transform.forward;
            if (!lockPlaneVec)
                planeMoveVec = mPlayerInput.CurrVec * moveSpeed;
        }


        //动画
        if (mAnimator)
        {
            float animSpeed = mPlayerInput.run ? 2 : 1;


            if (!cameraController.lockState)//未锁定目标
            {
                float val = Mathf.Lerp(mAnimator.GetFloat("forwordSpeed"), mPlayerInput.Dmag * animSpeed, 0.1f);
                mAnimator.SetFloat("forwordSpeed", val);
                mAnimator.SetFloat("rightSpeed", 0);
            }
            else
            {
                Vector3 localVec = transform.InverseTransformVector(mPlayerInput.CurrVec);
                float z = Mathf.Lerp(mAnimator.GetFloat("forwordSpeed"), localVec.z * animSpeed, 0.1f);
                mAnimator.SetFloat("forwordSpeed", z);
                float x = Mathf.Lerp(mAnimator.GetFloat("rightSpeed"), localVec.x * animSpeed, 0.1f);
                mAnimator.SetFloat("rightSpeed", x);
            }

            if (mPlayerInput.jump)
            {
                canAttackFlag = false;
                mAnimator.SetTrigger("jump");         
            }
            if (mPlayerInput.attack && CanAttack())
            {
                mAnimator.SetTrigger("attack");
            }

            mAnimator.SetBool("defence", mPlayerInput.denfence);
            if (mPlayerInput.roll || rigbody.velocity.magnitude > rollthreshold)
            {
                mAnimator.SetTrigger("roll");
                canAttackFlag = false;
            }
        }
    }

    void FixedUpdate()
    {
      //  rigbody.velocity = new Vector3(2, rigbody.velocity.y, 0) ;
        rigbody.position += deltaPos_Rm;
        rigbody.velocity = new Vector3(planeMoveVec.x, rigbody.velocity.y, planeMoveVec.z) + thrustVelocity;
        thrustVelocity = Vector3.zero;
        deltaPos_Rm = Vector3.zero;

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
        int layerIndex = mAnimator.GetLayerIndex("attack");
        LerpWeight(layerIndex, lerpTarget,0.05f);
    }
    public void OnAttack1hA_Update()
    {

        thrustVelocity = model.transform.forward * mAnimator.GetFloat("attack1hAVelocity");
   
        int layerIndex = mAnimator.GetLayerIndex("attack");
        LerpWeight(layerIndex, lerpTarget, 0.2f);
    }

    public void OnUpdateRootMotion(object obj)//rootmotion的移动量更新
    {
        //动画移动量累加，model最后播放动画完毕后更新给rig
        if (CheckState("attack1h_C","attack"))
        {    //相机跟随角色时，第三下攻击的rootmotion位置浮动过大，需要缓和

            deltaPos_Rm = (Vector3)obj * 0.4f;
        }
           


    }
    //插值权重
    void LerpWeight(int layerIndex,float target,float t)
    {
        float currWeight = mAnimator.GetLayerWeight(layerIndex);
        float currLerp = Mathf.Lerp(currWeight, target, t);
        mAnimator.SetLayerWeight(layerIndex, currLerp);
    }


}
