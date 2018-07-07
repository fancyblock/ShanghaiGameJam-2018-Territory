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
        for(int i = 0; i < mapData.width; i++)
        {
            for(int j = 0; j < mapData.height; j++)
            {
                MapTileData mapTileData = mapData.tiles[getTileIndex(i, j)];

                if(mapTileData.type != eTileType.None)
                {
                    GameObject go = new GameObject("tile_"+ i + "_" + j);
                    go.transform.parent = transform;
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = new Vector2(i*mapData.tileWidth/2.0f + j*mapData.tileWidth/2.0f, -i*mapData.tileHeight/2.0f + j*mapData.tileHeight/2.0f);
                    go.layer = gameObject.layer;

                    SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                    sr.sprite = tileData.GetTileSprite(mapTileData.type, mapTileData.initCountry);
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
        MapTileData mapTileData = mapData.tiles[getTileIndex(x, y)];

        if (mapTileData.initCountry != eCountry.A)
            return;

        if(mapTileData.type == eTileType.FactoryLand || mapTileData.type == eTileType.CoreLand)
        {
            signalPopupUI.Dispatch(eUI.MakeTroop);
        }
    }
}
