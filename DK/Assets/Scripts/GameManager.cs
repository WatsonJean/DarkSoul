using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public WeaponFactory weaponFactory;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        InitWeaponDB();
    }

    // Update is called once per frame
    void InitWeaponDB()
    {
        DataBase weaponData = new DataBase();
        weaponFactory = new WeaponFactory(weaponData);
    }
}
