using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    equment,
    forage,
    enable
}


public enum ItemUse
{
    sword,
    flashLight,
    mideice,
    Oxygen,
    booster,
    strong_booster,
    compass,
    deodrante
}

public struct BaseItem
{
    public ItemType type;
    public ItemUse use;
    public string name;
    public Sprite icon;
    public GameObject _object;
    public int praice;
    public int weight;
}
