using UnityEngine;
using System;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "GameJam/Create TroopData")]
public class TroopData :  ScriptableObject
{
    public List<TroopInfo> troopList;
}


[Serializable]
public class TroopInfo
{
    public eTroopType type;
    public eCountry country;
    public GameObject prefab;
}
