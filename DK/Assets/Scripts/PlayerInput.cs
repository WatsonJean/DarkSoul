using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : IUserInput
{

    ActionButton acBtn_attack = new ActionButton();
    ActionButton acBtn_run = new ActionButton();
    ActionButton acBtn_jump = new ActionButton();
    ActionButton acBtn_denfence = new ActionButton();
    ActionButton acBtn_roll = new ActionButton();
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
        attack = AttackInput();
        denfence = DenfenceInput();
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
        return acBtn_denfence.isPressing;
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

    protected virtual bool AttackInput()
    {
        if (!enableAttack)
            return false;
        acBtn_attack.Tick(Input.GetKey(key_attack));
        return acBtn_attack.isDown;
    }
}
