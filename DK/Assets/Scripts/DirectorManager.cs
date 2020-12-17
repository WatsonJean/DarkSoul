using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
[RequireComponent(typeof(PlayableDirector))]
public class DirectorManager : IActorManagerInterface
{
    PlayableDirector pd;
    public TimelineAsset timelineAsset_stab;

    void Awake()
    {
        pd = GetComponent<PlayableDirector>();
        pd.playOnAwake = false;
        pd.playableAsset = null;
    }
    private void Start()
    {
    
    }

    public void Play_Stab(string timelineName, ActorManager attacker, ActorManager receiver)
    {
        if (pd.playableAsset != null)//说明在播放中
            return;
        if (timelineName != "Stab_timeline")
            return;
        pd.playableAsset = Instantiate(timelineAsset_stab);
        TimelineAsset timeline = pd.playableAsset as TimelineAsset;
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track.name == "AttackerScriptTrack")
            {
                foreach (var clip in track.GetClips())
                {
                    StabPlayableClip stabClip = clip.asset as StabPlayableClip;
                    pd.SetReferenceValue(stabClip.actorMgr.exposedName, attacker);
                }

                //pd.SetGenericBinding(track, attacker);
            }
            else if (track.name == "ReceiverScriptTrack")
            {
                foreach (var clip in track.GetClips())
                {
                    StabPlayableClip stabClip = clip.asset as StabPlayableClip;
                    pd.SetReferenceValue(stabClip.actorMgr.exposedName, receiver);
                }
                // pd.SetGenericBinding(track, receiver);
            }
            else if (track.name == "attacker")
            {
                pd.SetGenericBinding(track, attacker.ac.mAnimator);
            }
            else if (track.name == "receiver")
            {
                pd.SetGenericBinding(track, receiver.ac.mAnimator);
            }
        }

        Play();
    }

    public void Play()
    {
        pd.extrapolationMode = DirectorWrapMode.None;
        pd.initialTime = 0;
        pd.Stop();
        pd.Evaluate();
        pd.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && gameObject.layer == LayerMask.NameToLayer("player"))
        {
            Play();
        }
    }
}
