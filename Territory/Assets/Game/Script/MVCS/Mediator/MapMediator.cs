using System;
using System.Collections;
using strange.extensions.mediation.impl;
using UnityEngine;


public class MapMediator : Mediator
{
    [Inject]
    public MapView view { get; set; }
    [Inject]
    public StartupSignal signalStartup { get; set; }
    [Inject]
    public MakeTroopSignal signalMakeTroop { get; set; }
    [Inject]
    public GameModel modelGame { get; set; }
    [Inject]
    public PlayerModel modelPlayer { get; set; }
    [Inject]
    public GameStatusChangeSignal signalGameStatusChange { get; set; }
    [Inject]
    public MapRefreshSignal signalMapRefresh { get; set; }
    [Inject]
    public PopupUISignal signalPopupUI { get; set; }

    private int curTileX, curTileY;
    private Troop curTroop;

    private bool noOperate = false;


    override public void OnRegister()
    {
        signalStartup.AddListener(onGameStart);
        signalMakeTroop.AddListener(onMakeTroop);
        signalMapRefresh.AddListener(onMapRefresh);
    }

    private void onMapRefresh()
    {
        // 部队的行动力恢复
        foreach(var tile in view.mapTiles.Values)
        {
            if (tile.troop)
                tile.troop.FINISH_ACTION = false;
        }
    }

    private void onGameStart()
    {
        view.CreateMap();
    }

    public void onTapTile(int x, int y)
    {
        if (noOperate)
            return;

        MapTileData mapTileData = view.mapData.tiles[view.getTileIndex(x, y)];

        // 移动部队或进攻
        if(curTroop)
        {
            if(curTroop.x == x && curTroop.y == y)
            {
                cancelSelectTroop(curTroop);
            }
            else
            {
                if(Math.Abs(curTroop.x - x) + Math.Abs(curTroop.y - y) == 1)
                {
                    MapTile mt = view.GetTile(x, y);

                    if(mt.troop)
                    {
                        //TODO 
                    }
                    else
                    {
                        // 移动到这一格
                        moveTroop(curTroop, x, y);
                    }
                }
            }
        }
        // 建造部队
        else if (mapTileData.initCountry == eCountry.A && (mapTileData.type == eTileType.FactoryLand || mapTileData.type == eTileType.CoreLand))
        {
            if (view.GetTile(x, y).troop)       // 上面有部队
                return;

            curTileX = x;
            curTileY = y;

            signalPopupUI.Dispatch(eUI.MakeTroop);
        }
    }

    public void onTapTroop(Troop troop)
    {
        if (noOperate)
            return;

        if (troop.FINISH_ACTION)
            return;

        if (curTroop == troop)
        {
            cancelSelectTroop(troop);
            return;
        }

        if (curTroop != null)
            return;

        view.ShowGridHint(troop.x, troop.y);
        troop.ShowOutline(true);

        curTroop = troop;
    }


    // 移动部队
    private void moveTroop(Troop troop, int x, int y)
    {
        StartCoroutine(movingTroop(troop, x, y));
    }

    private IEnumerator movingTroop(Troop troop, int x, int y)
    {
        noOperate = true;

        view.CloseGridHint();
        troop.ShowOutline(false);

        TweenPosition.Begin(troop.gameObject, 1.0f, view.GridToPosition(x, y));

        yield return new WaitForSeconds(1.0f);

        troop.sortingGroup.sortingOrder = view.GetTroopOrder(x, y);

        view.GetTile(troop.x, troop.y).troop = null;
        view.GetTile(x, y).troop = troop;

        troop.x = x;
        troop.y = y;

        troop.FINISH_ACTION = true;

        curTroop = null;

        noOperate = false;
    }

    private void cancelSelectTroop(Troop troop)
    {
        view.CloseGridHint();
        troop.ShowOutline(false);

        curTroop = null;
    }

    // 建造单位
    private void onMakeTroop(eTroopType troopType)
    {
        Debug.Log("make troop: " + troopType.ToString());

        int price = modelGame.GetTroopPrice(troopType);

        // 扣钱
        modelPlayer.COIN = modelPlayer.COIN - price;
        // 建造部队
        view.MakeTroop(troopType, curTileX, curTileY);
    }
}
