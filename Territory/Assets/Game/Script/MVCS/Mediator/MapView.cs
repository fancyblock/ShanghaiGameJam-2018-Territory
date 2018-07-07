using System;
using strange.extensions.mediation.impl;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class MapView : View
{
    [Inject]
    public PopupUISignal signalPopupUI { get; set; }
    [Inject]
    public GameModel modelGame { get; set; }
    [Inject]
    public PlayerModel modelPlayer { get; set; }


    public MapData mapData;
    public TileData tileData;
    public TroopData troopData;

    public int curTileX, curTileY;

    private Dictionary<string, MapTile> mapTiles = new Dictionary<string, MapTile>();


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
                    go.transform.localPosition = grid2position(i, j);
                    go.layer = gameObject.layer;

                    SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                    sr.sprite = tileData.GetTileSprite(mapTileData.type, mapTileData.initCountry);
                    sr.sortingOrder = getTileOrder(i, j);

                    go.AddComponent<PolygonCollider2D>();

                    MapTile tile = go.AddComponent<MapTile>();
                    tile.SetTile(i, j);
                    tile.SetTapCallback(onTapTile);

                    mapTiles.Add(i + "_" + j, tile);

                    if(mapTileData.type == eTileType.CoreLand)
                    {
                        if(mapTileData.initCountry == eCountry.A)
                        {
                            go = Instantiate(Resources.Load<GameObject>("baseA"), transform);
                            go.transform.localPosition = grid2position(i, j);
                            go.GetComponent<SortingGroup>().sortingOrder = getObjOrder(i, j);
                        }
                        else if(mapTileData.initCountry == eCountry.B)
                        {
                            go = Instantiate(Resources.Load<GameObject>("baseB"), transform);
                            go.transform.localPosition = grid2position(i, j);
                            go.GetComponent<SortingGroup>().sortingOrder = getObjOrder(i, j);
                        }
                    }
                }
            }
        }
    }


    private Vector2 grid2position(int x, int y)
    {
        return new Vector2(x * mapData.tileWidth / 2.0f + y * mapData.tileWidth / 2.0f, -x * mapData.tileHeight / 2.0f + y * mapData.tileHeight / 2.0f);
    }

    private int getTileOrder(int x, int y)
    {
        return 100 - (int)(-x * mapData.tileHeight / 2.0f + y * mapData.tileHeight / 2.0f);
    }

    private int getTroopOrder(int x, int y)
    {
        return 300 - (int)(-x * mapData.tileHeight / 2.0f + y * mapData.tileHeight / 2.0f) + 2;
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
            if (modelGame.IsFinishAction(x, y))     // 该回合已完成行动
            {
                // 弹框说明或显示正在建造中单位信息？

                return;
            }

            curTileX = x;
            curTileY = y;

            signalPopupUI.Dispatch(eUI.MakeTroop);
        }
    }

    public void MakeTroop(eTroopType troopType)
    {
        Debug.Log("make troop: " + troopType.ToString());

        int price = modelGame.GetTroopPrice(troopType);

        // 扣钱
        modelPlayer.COIN = modelPlayer.COIN - price;
        // 建造部队
        makeTroop(troopType, curTileX, curTileY);

        modelGame.MakeFinishAction(curTileX, curTileY);
    }


    private void makeTroop(eTroopType type, int x, int y)
    {
        TroopInfo ti = troopData.GetTroopInfo(type, eCountry.A);

        GameObject troopGo = Instantiate(ti.prefab, transform);
        troopGo.transform.localPosition = grid2position(x, y);

        Troop t = troopGo.GetComponent<Troop>();
        t.type = type;
        t.country = eCountry.A;
        t.x = x;
        t.y = y;

        t.SetTapCallback(onTapTroop);
        t.sortingGroup.sortingOrder = getTroopOrder(x, y);

        t.FINISH_ACTION = true;

        mapTiles[x + "_" + y].troop = t;
    }

    private void onTapTroop(Troop troop)
    {
        //TODO 
    }
}
