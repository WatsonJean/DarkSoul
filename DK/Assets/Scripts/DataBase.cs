using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponDataJson
{
    public List<WeaponDataModel> weaponDatas ;
}

[System.Serializable]
public class WeaponDataModel
{
    public float ATK;
    public string name;
}

public class DataBase 
{
    public Dictionary<string, WeaponDataModel> weaponDataDic;
    // Start is called before the first frame update
   public   DataBase()
    {
        weaponDataDic = new Dictionary<string, WeaponDataModel>();
        LoadJson();
    }

    // Update is called once per frame
    void LoadJson()
    {
      TextAsset textAsset =    Resources.Load("weaponJson") as TextAsset;
        WeaponDataJson json = JsonUtility.FromJson<WeaponDataJson>(textAsset.text);
        foreach (var item in json.weaponDatas)
        {
            if (!weaponDataDic.ContainsKey(item.name))
            {
                weaponDataDic.Add(item.name, item);
            }
        }        
    }
}
