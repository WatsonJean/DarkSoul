using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : IUserInput
{
    ActionButton acBtn_run = new ActionButton();
    ActionButton acBtn_action = new ActionButton();
    ActionButton acBtn_denfence = new ActionButton();
    ActionButton acLockTarget = new ActionButton();
    ActionButton acBtn_LT = new ActionButton();//重攻击
    ActionButton acBtn_RT = new ActionButton();
    ActionButton acBtn_LB = new ActionButton();//轻攻击
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

        UpdateVecDmag(inputX, inputY);

        run = RunInput();
        jump = JumpInput();
        roll = RollInput();
       
        lockTarget = LockTargetnput();
        LT = GetKeyDown(acBtn_LT, key_LT);// LTInput();
        RT = GetKeyDown(acBtn_RT, key_RT);//RTInput();
        LB = GetKeyDown(acBtn_LB, key_LB);//LBInput();  
        RB = GetKeyDown(acBtn_RB, key_RB);//RBInput();
        action = GetKeyDown(acBtn_action, key_action);
        denfence = acBtn_LB.isPressing; //必须在LB之后
    }

    protected virtual bool GetKeyDown(ActionButton actionButton,string keyname)
    {
        actionButton.Tick(Input.GetKey(keyname));
        return actionButton.isDown;
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

    protected virtual bool LockTargetnput()
    {
        if (!enableLockTarget)
            return false;
        acLockTarget.Tick(Input.GetKey(key_lockTarget));
        return acLockTarget.isDown;
    }
}
