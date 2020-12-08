using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : IUserInput
{

 //   ActionButton acBtn_attack = new ActionButton();
    ActionButton acBtn_run = new ActionButton();
    ActionButton acBtn_denfence = new ActionButton();
    ActionButton acLockTarget = new ActionButton();
    ActionButton acBtn_LT = new ActionButton();
    ActionButton acBtn_RT = new ActionButton();
    ActionButton acBtn_LB = new ActionButton();
    ActionButton acBtn_RB = new ActionButton();
    void Update()
    {

        if (enableMouseInput)
        {
            Input_ViewXY();
        }  
        Input_XY();
        if (enableInput == false)
        {
            inputX = 0;
            inputY = 0;
        }

        Vector2 temp = SquareToCircle(new Vector2(inputX, inputY));
        Dmag = Mathf.Sqrt(temp.x * temp.x + temp.y * temp.y);
        CurrVec = transform.right * temp.x + transform.forward * temp.y;

        run = RunInput();
        jump = JumpInput();
        roll = RollInput();
       // attack = AttackInput();
        denfence = DenfenceInput()? !denfence : denfence;
        lockTarget = LockTargetnput();
        LT = LTInput();
        LB = LBInput();
        RT = RTInput();
        RB = RBInput();
    }

    protected virtual bool LTInput()
    {
        acBtn_LT.Tick(Input.GetKey(key_LT));
        return acBtn_LT.isDown;
    }
    protected virtual bool LBInput()
    {
        acBtn_LB.Tick(Input.GetKey(key_LB));
        return acBtn_LB.isDown;
    }
    protected virtual bool RTInput()
    {
        acBtn_RT.Tick(Input.GetKey(key_RT));
        return acBtn_RT.isDown;
    }
    protected virtual bool RBInput()
    {
        acBtn_RB.Tick(Input.GetKey(key_RB));
        return acBtn_RB.isDown;
    }
    protected virtual void Input_XY()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
    }
    protected virtual void Input_ViewXY()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity_X;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity_Y;
    }
    protected virtual bool RollInput()
    {
        if (!enableRoll)
            return false;
        // acBtn_roll.Tick(Input.GetKey(key_roll));
        return acBtn_run.isUp && acBtn_run.isDelaying;
    }

    protected virtual bool DenfenceInput()
    {
        acBtn_denfence.Tick(Input.GetKey(key_denfence));
        return acBtn_denfence.isDown;
    }
    protected virtual bool RunInput()
    {
        if (!enableRun)
            return false;
        acBtn_run.Tick(Input.GetKey(key_run));

        return (acBtn_run.isPressing && !acBtn_run.isDelaying) || acBtn_run.isExtending;
    }

    protected virtual bool JumpInput()
    {
        if (!enableJump)
            return false;
        //acBtn_run.Tick(Input.GetKey(key_jump)); 
        return acBtn_run.isDown && acBtn_run.isExtending;//双击跳跃
    }

    //protected virtual bool AttackInput()
    //{
    //    if (!enableAttack)
    //        return false;
    //    acBtn_attack.Tick(Input.GetKey(key_attack));
    //    return acBtn_attack.isDown;
    //}

    protected virtual bool LockTargetnput()
    {
        if (!enableLockTarget)
            return false;
        acLockTarget.Tick(Input.GetKey(key_lockTarget));
        return acLockTarget.isDown;
    }
}
