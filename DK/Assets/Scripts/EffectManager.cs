using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : IActorManagerInterface
{
    // Start is called before the first frame update
    void Awake()
    {
        CloseAll();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseAll()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void SwitchEffect(string name,bool val)
    {
        foreach (Transform item in transform)
        {
            if (name == item.name)
            {

                item. gameObject.SetActive(val);
                break;
            }
        }

    }

    public GameObject InstantiateEffect(string name,Vector3 pos,Quaternion q )
    {
        GameObject go =Instantiate(  Resources.Load("Effects/"+ name) as GameObject);
        go.transform.position = pos;
        go.transform.rotation = q;
        return go;
        
    }
    public GameObject InstantiateEffect(GameObject obj,Vector3 pos, Quaternion q)
    {
        GameObject go = Instantiate(obj) as GameObject;
        go.transform.position = pos;
        go.transform.rotation = q;
        return go;
    }
}
