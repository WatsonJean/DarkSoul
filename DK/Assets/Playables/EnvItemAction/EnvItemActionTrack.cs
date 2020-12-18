using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0f, 0.7870975f, 1f)]
[TrackClipType(typeof(EnvItemActionClip))]
public class EnvItemActionTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<EnvItemActionMixerBehaviour>.Create (graph, inputCount);
    }
}
