using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEvent : IActorManagerInterface
{
    public string eventName;
    public bool active = true ;
    void Awake()
    {
        actorManager = GetComponentInParent<ActorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
