using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : IUserInput
{
    ActionButton acBtn_run = new ActionButton();
    ActionButton acBtn_action = new ActionButton();
    ActionButton acBtn_denfence = new ActionButton();
    ActionButton acLockTarget = new ActionButton();
    ActionButton acBtn_skill1 = new ActionButton();
    ActionButton acBtn_skill2 = new ActionButton();
    ActionButton acBtn_skill3 = new ActionButton();
    ActionButton acBtn_attack1 = new ActionButton();
    ActionButton acBtn_attack2 = new ActionButton();
    ActionButton acBtn_counterBack = new ActionButton();
    
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
        skill1 = GetKeyDown(acBtn_skill1, key_skill1);
        skill2 = GetKeyDown(acBtn_skill2, key_skill2);
        skill3 = GetKeyDown(acBtn_skill3, key_skill3);
        attack1 = GetKeyDown(acBtn_attack1, key_attack1);
        attack2 = GetKeyDown(acBtn_attack2, key_attack2);
        counterBack = GetKeyDown(acBtn_counterBack, key_counterBack);
        action = GetKeyDown(acBtn_action, key_action);
        denfence = acBtn_attack1.isPressing; //必须在LB之后
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
