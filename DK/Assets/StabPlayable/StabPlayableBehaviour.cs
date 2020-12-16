using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class StabPlayableBehaviour : PlayableBehaviour
{
    public GameObject newExposedReference;
    public float newBehaviourVariable;
    PlayableDirector pd;
    ActorManager ac;
    public override void OnPlayableCreate (Playable playable)
    {
        pd = playable.GetGraph().GetResolver() as PlayableDirector;
        //Debug.Log("OnPlayableCreate");
    }

    public override void OnGraphStart(Playable playable)
    {

        foreach (PlayableBinding item in pd.playableAsset.outputs)
        {
            if (item.streamName == "AttackerScriptTrack" || item.streamName == "ReceiverScriptTrack")
            {
                ActorManager ac = pd.GetGenericBinding(item.sourceObject) as ActorManager;
                ac.LockActorController(true);
                //ac.Die();
                
            }
        }
    }

    public override void OnGraphStop(Playable playable)
    {

        foreach (PlayableBinding item in pd.playableAsset.outputs)
        {
            if (item.streamName == "AttackerScriptTrack" || item.streamName == "ReceiverScriptTrack")
            {
                ActorManager ac = pd.GetGenericBinding(item.sourceObject) as ActorManager;
                ac.LockActorController(false);
            }
        }
    }
}
