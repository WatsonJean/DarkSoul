using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class EnvItemActionClip : PlayableAsset, ITimelineClipAsset
{
    public EnvItemActionBehaviour template = new EnvItemActionBehaviour ();
    public ExposedReference<EnvItemBase> EnvItem;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<EnvItemActionBehaviour>.Create (graph, template);
        EnvItemActionBehaviour clone = playable.GetBehaviour ();
        clone.EnvItem = EnvItem.Resolve (graph.GetResolver ());
        return playable;
    }
}
