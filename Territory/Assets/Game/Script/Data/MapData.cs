using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(menuName = "GameJam/Create MapData")]
public class MapData : ScriptableObject
{
    public int width;
    public int height;

    public float tileWidth;
    public float tileHeight;

    public MapTileData[] tiles;
}


[Serializable]
public class MapTileData
{
    public eTileType type;
    public eCountry initCountry;
}