using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class IUserInput : MonoBehaviour
{
    [Header("=============功能键位=============")]
    public string key_attack;
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
    public bool run = true;
    public bool jump = false;
    public bool lastJump = false;
    public bool attack = false;
    public bool lastAttack = false;

    public Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector3.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }
}
