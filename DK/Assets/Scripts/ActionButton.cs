using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton 
{
    public bool isDown;//按下
    public bool isUp;//按完抬起
    public bool isPressing;//按住
    public bool isDoubleClick;//双击
    public bool isExtending;//单次点击抬起后计时中状态
    public bool isDelaying;//单次点击按下后计时中状态
    bool currState;
    bool lastState;
    AcButtonTimer _ExtendingTimer = new AcButtonTimer();//松开后的延长计时
    AcButtonTimer _DelayingTimer = new AcButtonTimer();//按下后开始计时

    public void Tick(bool inputState)
    {
        _ExtendingTimer.Tick();
        _DelayingTimer.Tick();
        currState = isPressing = inputState;
        isDown = false;
        isUp = false;
        isDoubleClick = false;
        if (currState != lastState)
        {
            if (currState)
            {
                 isDown = true;
                _DelayingTimer.Start();
            }
            else
            {
                isUp = true;
                _ExtendingTimer.Start();
            }
        }
        lastState = currState;
        isDelaying = _DelayingTimer.currState == AcButtonTimer.State.Run;
        isExtending = _ExtendingTimer.currState == AcButtonTimer.State.Run;
        
    }
}
