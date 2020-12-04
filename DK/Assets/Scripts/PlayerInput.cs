using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : IUserInput
{

    void Update()
    {
        if (enableMouseInput)
        {
            base.Input_ViewXY();
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
        attack = AttackInput();
        denfence = DenfenceInput();
    }
}
