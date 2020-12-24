using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class EnvItemActionBehaviour : PlayableBehaviour
{
    public EnvItemBase EnvItem;
    public float newBehaviourVariable;
    PlayableDirector pd;

    public override void OnPlayableCreate(Playable playable)
    {
        pd = playable.GetGraph().GetResolver() as PlayableDirector;
        //Debug.Log("OnPlayableCreate");
    }


    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        EnvItem.Lock(true);

    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {

      
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        EnvItem.Lock(false);

    }
    public override void OnGraphStart(Playable playable)
    {

    }

    public override void OnGraphStop(Playable playable)
    {

    }
}
