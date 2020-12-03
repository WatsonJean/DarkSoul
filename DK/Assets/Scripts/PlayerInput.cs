using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : IUserInput
{

    void Awake()
    {

    }
    void Update()
    {         
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

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
        attack = AttackInput();

    }

    bool RunInput()
    {
        return enableRun ? Input.GetKey(key_run) : false;
    }

    bool JumpInput()
    {
        if (!enableJump)
            return false;

        bool newJump = Input.GetKey(key_jump);
        bool result = (newJump != lastJump && newJump) ? true : false;
        lastJump = newJump;
        return result;
    }

    bool AttackInput()
    {
        if (!enableAttack)
            return false;

        bool newAttack = Input.GetKey(key_attack);
        bool result = (newAttack != lastAttack && newAttack) ? true : false;
        lastAttack = newAttack;
        return result;
    }
}
