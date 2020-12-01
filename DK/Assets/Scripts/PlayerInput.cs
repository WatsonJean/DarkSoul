using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("=============功能键位=============")]
    public string key_attack ;
    public string key_run;
    public string key_jump;
    [Header("=============输入开关=============")]
    public bool enableAttack = true;
    public bool enableRun = true;
    public bool enableJump = true;
    public bool enableInput = true;
    [Header("=============输入信号=============")]
    public float inputX;
    public float inputY;
    public float mouseX;
    public float mouseY;
    [Header("=============其他=============")]
    public Vector3 CurrVec;
    public float Dmag;
    bool run = true;
    bool jump = false;
    bool lastJump = false;
    bool attack = false;
    bool lastAttack = false;

    public bool Runing()
    {
        return run;
    }

    public bool Jump()
    {
        return jump;
    }

    public bool Attack()
    {
        return attack;
    }
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

    Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector3.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
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
