using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIMediator : Mediator
{
    [Inject]
    public GameStatusChangeSignal signalGameStatusChange { get; set; }
    [Inject]
    public EndTurnSignal signalEndTurn { get; set; }
    [Inject]
    public GameModel modelGame { get; set; }
    [Inject]
    public MakeTroopSignal signalMakeTroop { get; set; }
    [Inject]
    public MoveTroopSignal signalMoveTroop { get; set; }


    override public void OnRegister()
    {
        signalGameStatusChange.AddListener(onGameStatusChange);
    }

    private void onGameStatusChange(eInGameStatus status)
    {
        if (status == eInGameStatus.BTurn)
            StartCoroutine(AITurn());
    }

    private IEnumerator AITurn()
    {
        yield return new WaitForSeconds(0.5f);

        List<Troop> troops = new List<Troop>();
        MapTile tileCoreBase = null;

        foreach(MapTile mt in modelGame.mapTiles.Values)
        {
            if (mt.troop && mt.troop.country == eCountry.B)
                troops.Add(mt.troop);

            if (mt.type == eTileType.CoreLand)
                tileCoreBase = mt;
        }

        troops.Sort((Troop t1, Troop t2) => 
        {
            int x1 = t1.x + t1.y;
            int x2 = t2.x + t2.y;
            return x1 - x2;
        });

        int troopCount = troops.Count;
        for(int i = 0; i < troopCount; i++)
        {
            Troop unit = troops[i];
            MapTile mt = getAvailabelStep(unit);

            if(mt != null)
            {
                signalMoveTroop.Dispatch(unit, mt.x, mt.y);

                while (MapMediator.noOperate)
                    yield return new WaitForSeconds(0.1f);

                yield return new WaitForSeconds(0.3f);
            }
        }

        // 是否建造？
        if (tileCoreBase.troop == null)
            signalMakeTroop.Dispatch(getNeededTroop(), eCountry.B, tileCoreBase.x, tileCoreBase.y);

        yield return new WaitForSeconds(0.3f);

        signalEndTurn.Dispatch(false);
    }


    private MapTile getAvailabelStep(Troop unit)
    {
        MapTile mt = null;

        List<string> keys = new List<string>();

        keys.Add((unit.x + 1) + "_" + unit.y);
        keys.Add((unit.x - 1) + "_" + unit.y);
        keys.Add(unit.x + "_" + (unit.y + 1));
        keys.Add(unit.x + "_" + (unit.y - 1));

        List<MapTile> tiles = new List<MapTile>();

        foreach(string key in keys)
        {
            MapTile tile = null;

            if(modelGame.mapTiles.TryGetValue(key, out tile))
                tiles.Add(tile);
        }

        tiles.Sort((MapTile t1, MapTile t2)=> 
        {
            int v1 = 0;
            int v2 = 0;

            if(t1.troop)
            {
                if (t1.troop.country == eCountry.A)
                    v1 += 1;
                else
                    v1 -= 1;
            }
            else
            {
                if (t1.type == eTileType.CrossLand)
                    v1 += 3;
                else
                    v1 += 2;
            }

            if (t2.troop)
            {
                if (t2.troop.country == eCountry.A)
                    v2 += 1;
                else
                    v2 -= 1;
            }
            else
            {
                if (t2.type == eTileType.CrossLand)
                    v2 += 3;
                else
                    v2 += 2;
            }

            return v2 - v1;
        });

        if (tiles[0].troop == null || tiles[0].troop.country != eCountry.B)
            mt = tiles[0];

        return mt;
    }

    private eTroopType getNeededTroop()
    {
        int val = Random.Range(0, 3);

        switch (val)
        {
            case 0:
                return eTroopType.paper;
            case 1:
                return eTroopType.rock;
            case 2:
                return eTroopType.scissors;
            default:
                break;
        }

        return eTroopType.paper;
    }
}
