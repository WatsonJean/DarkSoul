using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackTypeAnimatorFlag
{
    attack1=0,
    attack2,
    skill1 ,
    skill2,
    skill3
}


public class ActionController : MonoBehaviour
{
    public float runSpeed = 5;
    public float walkSpeed = 1.4f;
    public float JumpVelocity = 5f;
    public float rollVelocity = 1f;
    public float rollthreshold = 4;//判断是否能翻滚的阈值
    public float fallHeight = 0;
    public GameObject model;
    public CameraController cameraController;
    [Space(10)] 
    [Header("======  PhysicMaterial Setting ======")]
    public PhysicMaterial phy_Mat_zero;
    public PhysicMaterial phy_Mat_one;
    public Animator mAnimator;
    public IUserInput mInput;
    Rigidbody rigbody;
    CapsuleCollider capsuleCollider;
    Vector3 planeMoveVec;
    Vector3 thrustVelocity=Vector3.zero;//跳跃向前的速度
    Vector3 deltaPos_Rm; //rootmotion中动画的偏移量

    float lerpTarget;
    bool lockPlaneVec = false;
    bool lockDirection = false;
    bool isGround = false;
    bool canAttackFlag = true;
    public bool leftIsShield = false;
    public bool isAI = false;

    public delegate void OnActionDelegate();
    public event OnActionDelegate OnActionEvents;

    void Awake()
    {
        rigbody = GetComponent<Rigidbody>();
        mInput = GetComponent<IUserInput>();
        mAnimator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
    }

    void Start()
    {
    }

    void Update()
    {

        float moveSpeed = mInput.run ? runSpeed : walkSpeed;
        if (mInput.lockTarget)
        {
            cameraController.LockUnLock();
        }
        if ( ! cameraController.lockState)//未锁定目标
        {
            if (mInput.Dmag > 0.1 && mInput.enabled)
                model.transform.forward = Vector3.Slerp(model.transform.forward, mInput.CurrVec, 0.3f);
            if ( ! lockPlaneVec)
                planeMoveVec = model.transform.forward* mInput .Dmag* moveSpeed;
        }
        else //锁定目标状态
        {
            if (lockDirection) //进行跳跃翻滚等动画时要改变方向
            {
                model.transform.forward = planeMoveVec.normalized;
               
            }
            else
            {
                model.transform.forward = transform.forward;
            }
            if (!lockPlaneVec)
                planeMoveVec = mInput.CurrVec * moveSpeed;
        }

        //动画
        if (mAnimator)
        {
            LocalMotionBehaviour();
            FightBehaviour();

            if (mInput.action)
            {
                OnActionEvents.Invoke();
            }
        }
    }

    void LocalMotionBehaviour()
    {
        float animSpeed = mInput.run ? 2 : 1;
        if (!cameraController.lockState)//未锁定目标
        {
            float val = Mathf.Lerp(mAnimator.GetFloat("forwordSpeed"), mInput.Dmag * animSpeed, 0.1f);
            mAnimator.SetFloat("forwordSpeed", val);
            mAnimator.SetFloat("rightSpeed", 0);
        }
        else
        {
            Vector3 localVec = transform.InverseTransformVector(mInput.CurrVec);
            float z = Mathf.Lerp(mAnimator.GetFloat("forwordSpeed"), localVec.z * animSpeed, 0.1f);
            mAnimator.SetFloat("forwordSpeed", z);
            float x = Mathf.Lerp(mAnimator.GetFloat("rightSpeed"), localVec.x * animSpeed, 0.1f);
            mAnimator.SetFloat("rightSpeed", x);
        }
        //跳
        if (mInput.jump)
        {
            canAttackFlag = false;
            mAnimator.SetTrigger("jump");
        }
        //翻滚
        //if (mInput.roll || rigbody.velocity.magnitude > rollthreshold)
        if (mInput.roll)
        {
            mAnimator.SetTrigger("roll");
            canAttackFlag = false;
        }
        //后跳
        if (mInput.roll && mInput.Dmag < 0.1)
        {
            mAnimator.SetTrigger("jab");
            canAttackFlag = false;
        }
    }

    void FightBehaviour()
    {
        //举盾防御
        int layerIndex = mAnimator.GetLayerIndex("denfense");
        if (leftIsShield) //如果左手有盾
        {
            if (mInput.denfence)
            {
                if (CanDefence())
                {
                    mAnimator.SetBool("defence", true);
                    LerpWeight(layerIndex, 1, 0.3f);
                }
                else
                {
                    LerpWeight(layerIndex, 0, 0.2f);
                }
            }
            else
            {
                LerpWeight(layerIndex, 0, 0.2f);
                mAnimator.SetBool("defence", false);
            }
        }
        else
        {
            LerpWeight(layerIndex, 0, 0.2f);
        }

        //盾反
        if (mInput.counterBack && CanCounterBack() && leftIsShield)
        {
            mAnimator.SetTrigger("counterBack");
        }

        //轻重攻击
      else  if ((mInput.attack1 || mInput.attack2) && CanAttack())
        {
            AttackTypeAnimatorFlag type = mInput.attack1 ? AttackTypeAnimatorFlag.attack1 : AttackTypeAnimatorFlag.attack2;
            mAnimator.SetInteger("attackType", (int)type);
            mAnimator.SetTrigger("attack");
        }

        //技能
        else if ((mInput.skill1 || mInput.skill2 || mInput.skill3) && CanAttack())
        {
            AttackTypeAnimatorFlag type = mInput.skill1 ? AttackTypeAnimatorFlag.skill1 : mInput.skill2? AttackTypeAnimatorFlag.skill2: AttackTypeAnimatorFlag.skill3;
            mAnimator.SetInteger("attackType", (int)type);
            mAnimator.SetTrigger("attack");
        }
    }

    void FixedUpdate()
    {
        rigbody.position += deltaPos_Rm;
        rigbody.velocity = new Vector3(planeMoveVec.x, rigbody.velocity.y, planeMoveVec.z) + thrustVelocity;
        thrustVelocity = Vector3.zero;
        deltaPos_Rm = Vector3.zero;
    }

   public bool CheckState(string name,string layername = "Base Layer")
    {
        int layerIndex = mAnimator.GetLayerIndex(layername); 
        return mAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsName(name);
    }

    public bool CheckStateByTag(string name, string layername = "Base Layer")
    {
        int layerIndex = mAnimator.GetLayerIndex(layername);
        return mAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsTag(name);
    }
    bool CanAttack()
    {
        return (CheckState("Ground") ||  CheckStateByTag("attack") || CheckStateByTag("skill")) && canAttackFlag;
    }

    bool CanCounterBack()
    {
        return (CheckState("blocked") || CheckState("Ground") || CheckStateByTag("attack") || CheckStateByTag("skill")) && canAttackFlag;
    }

    bool CanDefence()
    {
        return CheckState("Ground")   ;
    }

    void OnJumpEnter()
    { 
        thrustVelocity = new Vector3(0, JumpVelocity, 0);
        mInput.enableInput = false;
         lockPlaneVec = true;
        lockDirection = true;
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
        mAnimator.SetFloat("fallHeight", 0);
        fallHeight = 0;
        mInput.enableInput = false;
        lockPlaneVec = true;
        canAttackFlag = false; 
    }

    public void OnFallUpdate()
    {
        mAnimator.SetFloat("fallHeight", rigbody.velocity.magnitude);
    }

    public void OnFallEnd()
    {
       // fallHeight = 0;
        planeMoveVec = Vector3.zero;
    }
    public void OnGroundEnter()
    {
        mInput.enableInput = true;
        lockPlaneVec = false;
        lockDirection = false;
        canAttackFlag = true;
        capsuleCollider.material = phy_Mat_one;
    }

    public void OnGroundExit()
    {
        capsuleCollider.material = phy_Mat_zero;
    }

    public void OnRollEnter()
    {
       // thrustVelocity = model.transform.forward * mAnimator.GetFloat("jabVelocity");
       // thrustVelocity = new Vector3(planeMoveVec.x, 0, planeMoveVec.z) * 50;
        //thrustVelocity =  new Vector3(0, rollVelocity, 0);
        mInput.enableInput = false;
        lockPlaneVec = true;
        lockDirection = true;
    }

    public void OnRollUpdate()
    {
        thrustVelocity = model.transform.forward * mAnimator.GetFloat("rollVelocity");

    }
    public void OnJabEnter()
    {
        mInput.enableInput = false;
        lockPlaneVec = true;
    }
    public void OnJabUpdate()
    {
     
        thrustVelocity = model.transform.forward * mAnimator.GetFloat("jabVelocity");
    }

    public void OnHitEnter()
    {
        mInput.enableInput = false;
        planeMoveVec = Vector3.zero;
        mAnimator.ResetTrigger("hit");
        object ob = 0;
        model.SendMessage("WeaponEnable", ob);
    }

    public void OnDieEnter()
    {
        mInput.enableInput = false;
        planeMoveVec = Vector3.zero;
        object ob = 0;
        model.SendMessage("WeaponEnable", ob);

    }


    //************************************************************************以下为攻击层级********************************************
    //*************************************************************************************************************************************

    public void OnAttack1hA_Enter()
    {
        lockPlaneVec = true;
        planeMoveVec = Vector3.zero;
        //mInput.enableInput = false;
        rigbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }


    public void OnUpdateRootMotion(object obj)//rootmotion的移动量更新
    {
        //动画移动量累加，model最后播放动画完毕后更新给rig
        if (CheckStateByTag("skill") || CheckStateByTag("attack"))
        {    //相机跟随角色时，第三下攻击的rootmotion位置浮动过大，需要缓和
            deltaPos_Rm = (Vector3)obj ;
        }         
    }

    //被盾反
    public void OnStunned_Enter()
    {
        mInput.enableInput = false;
        planeMoveVec = Vector3.zero;
    }

    public void OnCounterBack_Enter()
    {
        mInput.enableInput = false;
        planeMoveVec = Vector3.zero;
    }

    public void OnCounterBack_Exit()
    {
        object ob = 0;
        model.SendMessage("SetCounterBack", ob);
    }

    public void OnLockEnter()
    {
        mInput.enableInput = false;
        planeMoveVec = Vector3.zero;
    }

    public void OnSkillEnter()
    {
        // mInput.enableInput = false;
        lockPlaneVec = true;
        planeMoveVec = Vector3.zero;
        rigbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    public void OnSkillEnd()
    {
        rigbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
    public void OnAttack_Exit()
    {
        object ob = 0;
        model.SendMessage("WeaponEnable", ob);
        rigbody.constraints = RigidbodyConstraints.FreezeRotation;

    }
    //插值权重
    void LerpWeight(int layerIndex,float target,float t)
    {
        float currWeight = mAnimator.GetLayerWeight(layerIndex);
        float currLerp = Mathf.Lerp(currWeight, target, t);
        mAnimator.SetLayerWeight(layerIndex, currLerp);
    }

    public void IssueTrigger(string name)
    {
        mAnimator.SetTrigger(name);
    }

    public void SetBool(string key,bool val)
    {
        mAnimator.SetBool(key, val);
    }

    public void SetFloat(string key, float val)
    {
        mAnimator.SetFloat(key, val);
    }
}
