using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelDoor : MonoBehaviour
{
    public bool open = false;
    public Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            //transform.Translate(-Vector3.up * Time.deltaTime);
            transform.position = Vector3.Slerp(transform.position, target,0.01f);
        }
    }

    public void Open()
    {
        open = true;
        target = transform.position  - transform.up * 12f;
    }
}
