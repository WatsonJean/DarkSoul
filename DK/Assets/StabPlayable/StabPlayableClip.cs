using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class StabPlayableClip : PlayableAsset, ITimelineClipAsset
{
    public StabPlayableBehaviour template = new StabPlayableBehaviour ();
    public ExposedReference<GameObject> newExposedReference;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<StabPlayableBehaviour>.Create (graph, template);
        StabPlayableBehaviour clone = playable.GetBehaviour ();
        clone.newExposedReference = newExposedReference.Resolve (graph.GetResolver ());
        return playable;
    }
}
