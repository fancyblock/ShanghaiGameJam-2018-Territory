using UnityEngine;
using System;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "GameJam/Create TroopData")]
public class TroopData :  ScriptableObject
{
    public List<TroopInfo> troopList;


    public TroopInfo GetTroopInfo(eTroopType type, eCountry country)
    {
        foreach(TroopInfo ti in troopList)
        {
            if (ti.type == type && ti.country == country)
                return ti;
        }

        return null;
    }
}


[Serializable]
public class TroopInfo
{
    public eTroopType type;
    public eCountry country;
    public GameObject prefab;
}
