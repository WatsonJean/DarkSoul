using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effectobj : MonoBehaviour
{
    public ActorManager actorManager;
    public Vector3 offset;
    public Vector3 eulerAnglesOffset;
    public float time = 1f;
    public bool destroy = true;

    public float tempTime;

    void Start()
    {
        tempTime = 0;
        transform.position += offset;
        transform.eulerAngles += eulerAnglesOffset;
    }
    private void OnEnable()
    {
        tempTime = 0;
        transform.position += offset;
        transform.eulerAngles += eulerAnglesOffset;
    }
    // Update is called once per frame
    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime>= time)
        {
            DoThing();
            tempTime = 0;
        }
      
    }

    private void DoThing()
    {
        if (destroy)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
