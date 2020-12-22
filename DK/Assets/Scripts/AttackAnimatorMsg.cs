using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActionController))]
public class AttackAnimatorMsg : MonoBehaviour
{
    ActionController ac;
    // Start is called before the first frame update
    void Awake()
    {
        ac = GetComponent<ActionController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
