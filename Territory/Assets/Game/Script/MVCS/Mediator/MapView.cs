using System;
using strange.extensions.mediation.impl;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;


public class MapView : View
{
    public MapData mapData;
    public TileData tileData;
    public TroopData troopData;

    public Dictionary<string, MapTile> mapTiles = new Dictionary<string, MapTile>();


    public void CreateMap()
    {
        for(int i = 0; i < mapData.width; i++)
        {
            for(int j = 0; j < mapData.height; j++)
            {
                MapTileData mapTileData = mapData.tiles[getTileIndex(i, j)];

                if(mapTileData.type != eTileType.None)
                {
                    GameObject go = Instantiate(tileData.GetTilePrefab(mapTileData.type, mapTileData.initCountry), transform);
                    go.name = "tile_"+ i + "_" + j;
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = GridToPosition(i, j);

                    MapTile tile = go.GetComponent<MapTile>();
                    tile.SetOrder(getTileOrder(i, j));
                    
                    tile.SetTile(i, j);
                    tile.SetTapCallback(onTapTile);

                    mapTiles.Add(i + "_" + j, tile);

                    if(mapTileData.type == eTileType.CoreLand)
                    {
                        if(mapTileData.initCountry == eCountry.A)
                        {
                            go = Instantiate(Resources.Load<GameObject>("baseA"), transform);
                            go.transform.localPosition = GridToPosition(i, j);
                            go.GetComponent<SortingGroup>().sortingOrder = getObjOrder(i, j);
                        }
                        else if(mapTileData.initCountry == eCountry.B)
                        {
                            go = Instantiate(Resources.Load<GameObject>("baseB"), transform);
                            go.transform.localPosition = GridToPosition(i, j);
                            go.GetComponent<SortingGroup>().sortingOrder = getObjOrder(i, j);
                        }
                    }
                }
            }
        }
    }

    public void ShowGridHint(int x, int y)
    {
        for(int i = 0; i < mapData.width; i++)
        {
            for(int j = 0; j < mapData.height; j++)
            {
                MapTile mt = GetTile(i, j);

                if (mt != null)
                {
                    if (i == x && j == y)
                        continue;

                    if( Math.Abs(i-x) + Math.Abs(j-y) == 1 )
                    {
                        if(mt.troop)
                        {
                            if (mt.troop.country == eCountry.B)
                                mt.ShowAttack();
                            else
                                mt.ShowGrey();
                        }
                        else
                        {
                            mt.ShowMove();
                        }
                    }
                    else
                    {
                        mt.ShowGrey();
                    }
                }
            }
        }
    }

    public void CloseGridHint()
    {
        for (int i = 0; i < mapData.width; i++)
        {
            for (int j = 0; j < mapData.height; j++)
            {
                MapTile mt = GetTile(i, j);

                if(mt != null)
                {
                    mt.CloseHints();
                }
            }
        }
    }

    public MapTile GetTile(int x, int y)
    {
        string key = x + "_" + y;

        MapTile mt = null;

        mapTiles.TryGetValue(key, out mt);

        return mt;
    }

    public Vector2 GridToPosition(int x, int y)
    {
        return new Vector2(x * mapData.tileWidth / 2.0f + y * mapData.tileWidth / 2.0f, -x * mapData.tileHeight / 2.0f + y * mapData.tileHeight / 2.0f);
    }

    public int GetTroopOrder(int x, int y)
    {
        return 300 - (int)(-x * mapData.tileHeight / 2.0f + y * mapData.tileHeight / 2.0f) + 2;
    }


    private int getTileOrder(int x, int y)
    {
        return 100 - (int)(-x * mapData.tileHeight / 2.0f + y * mapData.tileHeight / 2.0f);
    }

    private int getObjOrder(int x, int y)
    {
        return 300 - (int)(-x * mapData.tileHeight / 2.0f + y * mapData.tileHeight / 2.0f);
    }

    public int getTileIndex(int x, int y)
    {
        return y * mapData.width + x;
    }

    private void onTapTile(int x, int y)
    {
        GetComponent<MapMediator>().onTapTile(x, y);
    }


    public void MakeTroop(eTroopType type, int x, int y)
    {
        TroopInfo ti = troopData.GetTroopInfo(type, eCountry.A);

        GameObject troopGo = Instantiate(ti.prefab, transform);
        troopGo.transform.localPosition = GridToPosition(x, y);

        Troop t = troopGo.GetComponent<Troop>();
        t.type = type;
        t.country = eCountry.A;
        t.x = x;
        t.y = y;

        t.SetTapCallback(onTapTroop);
        t.sortingGroup.sortingOrder = GetTroopOrder(x, y);

        t.FINISH_ACTION = true;

        mapTiles[x + "_" + y].troop = t;
    }

    private void onTapTroop(Troop troop)
    {
        GetComponent<MapMediator>().onTapTroop(troop);
    }
}
