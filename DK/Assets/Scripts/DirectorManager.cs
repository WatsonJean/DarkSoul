using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
[RequireComponent(typeof(PlayableDirector))]
public class DirectorManager : IActorManagerInterface
{
    public PlayableDirector pd;
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

    TimelineAsset LoadTimelineAsset(string name)
    {
        TimelineAsset timelineAsset = Resources.Load("Timeline/" + name) as TimelineAsset;
        return timelineAsset;
    }
    public void Play_ItemAction(string timelineName,  EnvItemBase box,Action action = null)
    {

        TimelineAsset timeline = LoadTimelineAsset(timelineName);// Instantiate(timelineAsset_stab);
        pd.playableAsset = timeline;
       ActorManager  attacker = box.trigger;
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track.name == "Item script")
            {
                foreach (var clip in track.GetClips())
                {

                    EnvItemActionClip eaclip = clip.asset as EnvItemActionClip;
                    eaclip.EnvItem.exposedName = System.Guid.NewGuid().ToString();
                    //print("eaclip.EnvItem.exposedName:"+ eaclip.EnvItem.exposedName);
                
                    pd.SetReferenceValue(eaclip.EnvItem.exposedName, box);
                }
            }
            else if (track.name == "player script")
            {
                foreach (var clip in track.GetClips())
                {

                    StabPlayableClip stabClip = clip.asset as StabPlayableClip;
                    stabClip.actorMgr.exposedName = System.Guid.NewGuid().ToString();
                    stabClip.EndEvent = action;
                    // print("stabClip.actorMgr.exposedName:" + stabClip.actorMgr.exposedName);
                    pd.SetReferenceValue(stabClip.actorMgr.exposedName, attacker);
                }
            }
            else if (track.name == "player Track")
            {
                pd.SetGenericBinding(track, attacker.ac.mAnimator);
            }
            else if (track.name == "Item Track")
            {
                pd.SetGenericBinding(track, box.mAnimator);
            }
        }

        Play();
    }
    public void Play_Stab(string timelineName, ActorManager attacker, ActorManager receiver,Action action = null)
    {

        pd.playableAsset = LoadTimelineAsset(timelineName);
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
                    receiver.Die();
                    stabClip.EndEvent = () => { receiver.ac.mAnimator.enabled = false; };
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
