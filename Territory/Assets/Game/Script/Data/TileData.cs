using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GameJam/Create TileData")]
public class TileData : ScriptableObject
{
    public List<TileInfo> tileList;
}


[Serializable]
public class TileInfo
{
    public eTileType type;
    public Sprite sprite;
}
