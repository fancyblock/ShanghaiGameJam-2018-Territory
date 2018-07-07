using System;
using strange.extensions.mediation.impl;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Collections;


public class MapView : View
{
    public MapData mapData;
    public TileData tileData;
    public TroopData troopData;

    public AnimatorPlayer animCloud;
    public AnimatorPlayer animLight;
    public AnimatorPlayer animCoin;


    public void CreateMap()
    {
        for(int i = 0; i < mapData.width; i++)
        {
            for(int j = 0; j < mapData.height; j++)
            {
                MapTileData mapTileData = mapData.tiles[getTileIndex(i, j)];

                if(mapTileData.type != eTileType.None)
                {
                    createTile(i, j, mapTileData.initCountry, mapTileData);
                }
            }
        }
    }

    public void ChangeTile(int x, int y, eCountry country)
    {
        Destroy(GetTile(x, y).gameObject);

        MapTileData mapTileData = mapData.tiles[getTileIndex(x, y)];
        createTile(x, y, country, mapTileData);
    }

    private void createTile(int x, int y, eCountry country, MapTileData mapTileData)
    {
        GameObject go = Instantiate(tileData.GetTilePrefab(country), transform);
        go.name = "tile_" + x + "_" + y;
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = GridToPosition(x, y);

        MapTile tile = go.GetComponent<MapTile>();
        tile.type = mapTileData.type;
        tile.country = country;

        tile.SetOrder(getTileOrder(x, y));

        tile.SetTile(x, y);
        tile.SetTapCallback(onTapTile);

        mapTiles[x + "_" + y] = tile;

        if (mapTileData.type == eTileType.CoreLand)
        {
            if (mapTileData.initCountry == eCountry.A)
            {
                go = Instantiate(Resources.Load<GameObject>("baseA"), transform);
                go.transform.localPosition = GridToPosition(x, y);
                go.GetComponent<SortingGroup>().sortingOrder = getObjOrder(x, y);
            }
            else if (mapTileData.initCountry == eCountry.B)
            {
                go = Instantiate(Resources.Load<GameObject>("baseB"), transform);
                go.transform.localPosition = GridToPosition(x, y);
                go.GetComponent<SortingGroup>().sortingOrder = getObjOrder(x, y);
            }
        }
    }

    public void PlayCloudAnim(int x, int y)
    {
        animCloud.transform.localPosition = GridToPosition(x, y);
        animCloud.gameObject.SetActive(true);

        StartCoroutine(playingAnim());
    }

    public void PlayLightAnim()
    {
        animLight.gameObject.SetActive(true);

        StartCoroutine(playingLight());
    }

    private IEnumerator playingLight()
    {
        animLight.Play("light");

        yield return new WaitForSeconds(0.9f);

        animLight.gameObject.SetActive(false);
    }

    private IEnumerator playingAnim()
    {
        animCloud.Play("boom");

        yield return new WaitForSeconds(2.0f);

        animCloud.gameObject.SetActive(false);
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
        return new Vector2(x * mapData.tileWidth / 2.0f + y * mapData.tileWidth / 2.0f, x * mapData.tileHeight / 2.0f - y * mapData.tileHeight / 2.0f);
    }

    public int GetTroopOrder(int x, int y)
    {
        return 300 - (int)(x * mapData.tileHeight / 2.0f - y * mapData.tileHeight / 2.0f) + 2;
    }


    private int getTileOrder(int x, int y)
    {
        return 100 - (int)(x * mapData.tileHeight / 2.0f - y * mapData.tileHeight / 2.0f);
    }

    private int getObjOrder(int x, int y)
    {
        return 300 - (int)(x * mapData.tileHeight / 2.0f - y * mapData.tileHeight / 2.0f);
    }

    public int getTileIndex(int x, int y)
    {
        return y * mapData.width + x;
    }

    private void onTapTile(int x, int y)
    {
        GetComponent<MapMediator>().onTapTile(x, y);
    }


    public void MakeTroop(eTroopType type, eCountry country, int x, int y)
    {
        TroopInfo ti = troopData.GetTroopInfo(type, country);

        GameObject troopGo = Instantiate(ti.prefab, transform);
        troopGo.transform.localPosition = GridToPosition(x, y);

        Troop t = troopGo.GetComponent<Troop>();
        t.type = type;
        t.country = country;
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


    private Dictionary<string, MapTile> mapTilesCache = null;

    private Dictionary<string, MapTile> mapTiles
    {
        get
        {
            if (mapTilesCache == null)
                mapTilesCache = GetComponent<MapMediator>().modelGame.mapTiles;
            
            return mapTilesCache;
        }
    }
}
