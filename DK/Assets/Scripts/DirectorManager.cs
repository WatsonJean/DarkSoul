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

    public ActorManager attacker;
    public ActorManager receiver;
    void Awake()
    {
        pd = GetComponent<PlayableDirector>();
        pd.playOnAwake = false;
       
    }
    private void Start()
    {
        SetTimeline_Stab();
    }

    public void SetTimeline_Stab()
    {
        pd.playableAsset = Instantiate(timelineAsset_stab);
        foreach (PlayableBinding track in pd.playableAsset.outputs)
        {
            if (track.streamName == "AttackerScriptTrack")
            {
                pd.SetGenericBinding(track.sourceObject, attacker);
            }
            else if (track.streamName == "ReceiverScriptTrack")
            {
                pd.SetGenericBinding(track.sourceObject, receiver);
            }
            else if (track.streamName == "attacker")
            {
                pd.SetGenericBinding(track.sourceObject, attacker.ac.mAnimator);
            }
            else if (track.streamName == "receiver")
            {
                pd.SetGenericBinding(track.sourceObject, receiver.ac.mAnimator);
            }
        }
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
