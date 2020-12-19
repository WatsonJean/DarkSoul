using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0f, 0.9811321f, 0.1708758f)]
[TrackClipType(typeof(StabPlayableClip))]
//[TrackBindingType(typeof(ActorManager))]
public class StabPlayableTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<StabPlayableMixerBehaviour>.Create (graph, inputCount);
    }
}
