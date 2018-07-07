using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GameJam/Create TileData")]
public class TileData : ScriptableObject
{
    public List<TileInfo> tileList;


    public GameObject GetTilePrefab(eTileType type, eCountry country)
    {
        // 只按照阵营划分
        foreach(TileInfo ti in tileList)
        {
            if (ti.country == country)
                return ti.prefab;
        }

        return null;
    }
}


[Serializable]
public class TileInfo
{
    public eTileType type;
    public eCountry country;
    public GameObject prefab;
}
