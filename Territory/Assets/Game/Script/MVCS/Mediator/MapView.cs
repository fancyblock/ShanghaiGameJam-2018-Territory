using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapView : View
{
    public MapData mapData;
    public TileData tileData;


    public void CreateMap()
    {
        Dictionary<eTileType, TileInfo> tileInfoDic = new Dictionary<eTileType, TileInfo>();

        foreach (TileInfo ti in tileData.tileList)
            tileInfoDic.Add(ti.type, ti);

        for(int i = 0; i < mapData.width; i++)
        {
            for(int j = 0; j < mapData.height; j++)
            {
                int index = j * mapData.width + i;

                eTileType tileType = mapData.tiles[index];

                if(tileType != eTileType.None)
                {
                    GameObject go = new GameObject("tile_"+ i + "_" + j);
                    go.transform.parent = transform;
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = new Vector2(i*mapData.tileWidth/2.0f + j*mapData.tileWidth/2.0f, -i*mapData.tileHeight/2.0f + j*mapData.tileHeight/2.0f);
                    go.layer = gameObject.layer;

                    SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                    sr.sprite = tileInfoDic[tileType].sprite;
                }
            }
        }
    }
}
