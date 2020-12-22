using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactory 
{
     DataBase dataBase;
    // Start is called before the first frame update
    public WeaponFactory(DataBase db)
    {
        dataBase = db;
    }
    public GameObject CreateWeapon(string name, Transform parent)
    {
        GameObject obj = Resources.Load("weapon/" + name) as GameObject;
        if (!obj)
            return null;
        GameObject wp = GameObject.Instantiate(obj);
        if (parent != null)
        {
            wp.transform.parent = parent;
        }

        SetData(name, wp);
        return wp;
    }
    // Update is called once per frame
    public GameObject CreateWeapon(string name,Transform parent, Vector3 pos,Quaternion rotation )
    {
        GameObject obj = Resources.Load("weapon/" + name) as GameObject;
        if (!obj)
            return null;
        GameObject wp = GameObject.Instantiate(obj);
        if (parent!=null)
        {
            wp.transform.parent = parent;
            wp.transform.localPosition = pos;
            wp.transform.localRotation = rotation;
        }
        else
        {
            wp.transform.position = pos;
            wp.transform.rotation = rotation;
        }

        SetData(name,wp);
        return wp;
    }

    public void SetData(string name,GameObject obj)
    {
        WeaponData data = obj.GetComponent<WeaponData>();
        if (data == null)
        {
            data = obj.AddComponent<WeaponData>();
        }

        if (dataBase.weaponDataDic.ContainsKey(name))
        {
            data.SetData(dataBase.weaponDataDic[name]);
        }
        else
            Debug.Log(name + " is null");
    }
}
