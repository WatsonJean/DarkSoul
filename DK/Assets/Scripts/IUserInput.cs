﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class IUserInput : MonoBehaviour
{
    [Header("=============功能键位=============")]
    //public string key_attack;
    public string key_run;
    public string key_jump;
    public string key_roll;
    public string key_denfence  ;
    public string key_lockTarget;
    public string key_skill1;
    public string key_skill2;
    public string key_skill3;
    public string key_attack1;
    public string key_attack2;
    public string key_counterBack;
    public string key_action;
    [Header("=============输入开关=============")]
    public bool enableLockTarget = true;
    public bool enableAttack = true;
    public bool enableRun = true;
    public bool enableJump = true;
    public bool enableRoll = true;
    public bool enableInput = true;
    public bool enableMouseInput = true;
    [Header("=============输出信号=============")]
    public float inputX;
    public float inputY;
    public float mouseX;
    public float mouseY;
    public float mouseSensitivity_X = 1f;
    public float mouseSensitivity_Y = 1f;
    public Vector3 CurrVec;
    public float Dmag;
    [Header("=============其他=============")]

    //-------pressing signal
    public bool run = false;
    public bool denfence = false;
    //-------trigger signal
    public bool jump = false;
    public bool action = false;
    public bool roll = false;
    public bool counterBack = false;
    public bool skill1 = false;
    public bool skill2 = false;
    public bool skill3 = false;
    public bool attack1 = false;
    public bool attack2 = false;
    public bool lockTarget = false;
    //---------other

    public Vector2 SquareToCircle(Vector2 input)
    {
 
        Vector2 output = Vector3.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }
   
    public void UpdateVecDmag(float right,float up)
    {
        Vector2 temp = SquareToCircle(new Vector2(right, up));
        Dmag = Mathf.Sqrt(temp.x * temp.x + temp.y * temp.y);
        CurrVec = transform.right * temp.x + transform.forward * temp.y;
    }

}
