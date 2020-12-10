using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelper 
{

   public static Transform DeepFind(this Transform parent,string name)
    {
        Transform temp = null;
        foreach (Transform item in parent)
        {
            if (item.name == name)
                return item;
            else
            {
                temp = DeepFind(item, name);
                if (temp!=null)
                {
                    return temp;
                }
            }
           
        }
        return null;
    }

}
