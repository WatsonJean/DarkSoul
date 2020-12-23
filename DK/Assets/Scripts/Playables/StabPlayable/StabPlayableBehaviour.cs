using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class StabPlayableBehaviour : PlayableBehaviour
{
    public ActorManager actorMgr;
    public float newBehaviourVariable;
    PlayableDirector pd;
    public Action EndEvent;
    public override void OnPlayableCreate (Playable playable)
    {
        pd = playable.GetGraph().GetResolver() as PlayableDirector;
        //Debug.Log("OnPlayableCreate");
    }


    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
     
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {

    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {

    }
    public override void OnGraphStart(Playable playable)
    {
        actorMgr.ac.mAnimator.enabled = true;
    }

    public override void OnGraphStop(Playable playable)
    {
        EndEvent?.Invoke();
    }
}
