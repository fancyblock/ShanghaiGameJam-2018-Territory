using System;
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
                //TODO 
            }
        }
        // 建造部队
        if (mapTileData.initCountry == eCountry.A && (mapTileData.type == eTileType.FactoryLand || mapTileData.type == eTileType.CoreLand))
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
        //TODO 

        curTroop = troop;
    }

    private void cancelSelectTroop(Troop troop)
    {
        view.CloseGridHint();
        //TODO 

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
