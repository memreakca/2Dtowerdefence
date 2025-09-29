using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hero 
{
    public string heroName;
    public int cost;
    public GameObject prefab;

    public Hero(string _name, int _cost, GameObject _prefab)
    {
        heroName = _name;
        cost = _cost;
        prefab = _prefab;
    }
}
