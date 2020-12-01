using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    public ActionController actionController;
    public CapsuleCollider capsuleCollider;
    public float offset = 0;
    Vector3 poinStart;
    Vector3 poinEnd;
    float radius;
    // Start is called before the first frame update
    void Awake()
    {

        radius = capsuleCollider.radius-0.05f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        poinStart = transform.position + transform.up * (radius + offset);
        poinEnd = transform.position + transform.up * capsuleCollider.height - transform.up * (radius+ offset);
        int mask = LayerMask.GetMask("ground");
        Collider[] outColliders = Physics.OverlapCapsule(poinStart, poinEnd, radius, mask);
        if(outColliders.Length>0)
        {
            
            actionController.IsGround();
        }
        else
        {
            actionController.IsNotGround();
        }
    }
}
