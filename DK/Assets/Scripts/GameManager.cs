﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public WeaponFactory weaponFactory;
    public levelDoor door;

    private void Awake()
    {
        instance = this;
        InitWeaponDB();
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void InitWeaponDB()
    {
        DataBase weaponData = new DataBase();
        weaponFactory = new WeaponFactory(weaponData);
    }

    public void OpenDoor()
    {
        door.Open();
    }
}
