using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenceUI : MonoBehaviour
{
    public static ScenceUI instance;
    public ActorManager actorManager;
    public Text hp;
    public Text damage;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        hp.text = "HP: " + actorManager.attributeMgr.HP;
      //  damage.text = "Attack: " + actorManager.weaponMgr..HP;
    }
}
