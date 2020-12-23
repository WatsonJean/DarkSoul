using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyInput : IUserInput
{
    // Start is called before the first frame update
    void Start()
    {
       // inputX = 0;
        //inputY =0;
        //attack2 = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVecDmag(inputX, inputY);
    }
}
