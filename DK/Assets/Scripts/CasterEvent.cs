using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEvent : IActorManagerInterface
{
    public string eventName;
    public bool active = true ;
    public EnvItemBase itemBase;

    void Awake()
    {
        actorManager = GetComponentInParent<ActorManager>();
        itemBase = GetComponentInParent<EnvItemBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
