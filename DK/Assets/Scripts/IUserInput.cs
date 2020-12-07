using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class IUserInput : MonoBehaviour
{
    [Header("=============功能键位=============")]
    public string key_attack;
    public string key_run;
    public string key_jump;
    public string key_roll;
    public string key_denfence  ;
    public string key_lockTarget;

    [Header("=============输入开关=============")]
    public bool enableLockTarget = true;
    public bool enableAttack = true;
    public bool enableRun = true;
    public bool enableJump = true;
    public bool enableRoll = true;
    public bool enableInput = true;
    public bool enableMouseInput = true;
    [Header("=============输入信号=============")]
    public float inputX;
    public float inputY;
    public float mouseX;
    public float mouseY;
    public float mouseSensitivity_X = 1f;
    public float mouseSensitivity_Y = 1f;
    [Header("=============其他=============")]
    public Vector3 CurrVec;
    public float Dmag;
    public bool run = true;
    public bool jump = false;
    public bool attack = false;
    public bool roll = false;
    public bool denfence = false;
    public bool lockTarget = false;
    bool lastJump = false;
    bool lastAttack = false;


    public Vector2 SquareToCircle(Vector2 input)
    {
 
        Vector2 output = Vector3.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }
   

}
