using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcButtonTimer 
{
    public enum State
    {
        Idle,
        Run,
        Finish
    }

    public float duration = 1.0f;
    public float elapsedTime= 0;
    public State currState = State.Idle;

    public void Start (float duration = 0.3f)
    {
        this.duration = duration;
        elapsedTime = 0;
        currState = State.Run;
    }

    public void Tick()
    {
        switch (currState)
        {
            case State.Idle:
                break;
            case State.Run:
                elapsedTime += Time.deltaTime;
                if (elapsedTime>duration)
                {
                    currState = State.Finish;
                }
                break;
            case State.Finish:
                break;
            default:
                break;
        }
    }
}
