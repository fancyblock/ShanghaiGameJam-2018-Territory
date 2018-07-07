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

        if (mapTileData.initCountry != eCountry.A)
            return;

        if (mapTileData.type == eTileType.FactoryLand || mapTileData.type == eTileType.CoreLand)
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

    public void onTapTroop(Troop troop)
    {
        //TODO 
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

        modelGame.MakeFinishAction(curTileX, curTileY);
    }
}
