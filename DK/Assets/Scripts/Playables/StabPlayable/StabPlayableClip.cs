using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class StabPlayableClip : PlayableAsset, ITimelineClipAsset
{
    public StabPlayableBehaviour template = new StabPlayableBehaviour ();
    public ExposedReference<ActorManager> actorMgr;
    public Action EndEvent;
    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<StabPlayableBehaviour>.Create (graph, template);
        StabPlayableBehaviour clone = playable.GetBehaviour ();
        clone.EndEvent = EndEvent;
        clone.actorMgr = actorMgr.Resolve (graph.GetResolver ());
        return playable;
    }
}
