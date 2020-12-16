using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DirectorManager : MonoBehaviour
{
   public PlayableDirector pd;
    public Animator attacker;
    public Animator receiver;
    void Awake()
    {
        pd = GetComponent<PlayableDirector>();
    }

    public void Bind(Animator attacker, Animator receiver)
    {
        foreach (PlayableBinding track in pd.playableAsset.outputs)
        {
            if (track.streamName == "attacker")
            {
                pd.SetGenericBinding(track.sourceObject, attacker);
            }
            else if (track.streamName == "receiver")
            {
                pd.SetGenericBinding(track.sourceObject, receiver);
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
        
    }
}
