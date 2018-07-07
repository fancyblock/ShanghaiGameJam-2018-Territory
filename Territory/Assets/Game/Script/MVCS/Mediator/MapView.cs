using strange.extensions.mediation.impl;
using System.Collections.Generic;
using UnityEngine;


public class MapView : View
{
    [Inject]
    public PopupUISignal signalPopupUI { get; set; }


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
                eTileType tileType = mapData.tiles[getTileIndex(i, j)];

                if(tileType != eTileType.None)
                {
                    GameObject go = new GameObject("tile_"+ i + "_" + j);
                    go.transform.parent = transform;
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = new Vector2(i*mapData.tileWidth/2.0f + j*mapData.tileWidth/2.0f, -i*mapData.tileHeight/2.0f + j*mapData.tileHeight/2.0f);
                    go.layer = gameObject.layer;

                    SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                    sr.sprite = tileInfoDic[tileType].sprite;
                    sr.sortingOrder = getTileOrder(i, j);

                    go.AddComponent<PolygonCollider2D>();

                    MapTile tile = go.AddComponent<MapTile>();
                    tile.SetTile(i, j);
                    tile.SetTapCallback(onTapTile);
                }
            }
        }
    }


    private int getTileOrder(int x, int y)
    {
        return 100 - (int)(-x * mapData.tileHeight / 2.0f + y * mapData.tileHeight / 2.0f);
    }

    private int getObjOrder(int x, int y)
    {
        return 300 - (int)(-x * mapData.tileHeight / 2.0f + y * mapData.tileHeight / 2.0f);
    }

    private int getTileIndex(int x, int y)
    {
        return y * mapData.width + x;
    }

    private void onTapTile(int x, int y)
    {
        eTileType tileType = mapData.tiles[getTileIndex(x, y)];

        if(tileType == eTileType.FactoryLand || tileType == eTileType.CoreLand)
        {
            signalPopupUI.Dispatch(eUI.MakeTroop);
        }
    }
}
