using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterActorManager : IActorManagerInterface
{
    CapsuleCollider capsuleCollider;
    public List<CasterEvent> casterEventsList = new List<CasterEvent>();
    void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CasterEvent[] ces = other.GetComponents<CasterEvent>();
        foreach (CasterEvent item in ces)
        {
            if (!casterEventsList.Contains(item))
            {
                casterEventsList.Add(item);
            }
        }
    }
    // Update is called once per frame
    private void OnTriggerExit(Collider other)
    {
        CasterEvent[] ces = other.GetComponents<CasterEvent>();
        foreach (CasterEvent item in ces)
        {
            if (casterEventsList.Contains(item))
            {
                casterEventsList.Remove(item);
            }
        }
    }
}
