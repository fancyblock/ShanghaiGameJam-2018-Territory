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


    override public void OnRegister()
    {
        signalStartup.AddListener(onGameStart);
        signalMakeTroop.AddListener(onMakeTroop);
    }


    private void onGameStart()
    {
        view.CreateMap();
    }

    // 建造单位
    private void onMakeTroop(eTroopType troopType)
    {
        Debug.Log("make troop: " + troopType.ToString());

        int price = modelGame.GetTroopPrice(troopType);

        // 扣钱
        modelPlayer.COIN = modelPlayer.COIN - price;
        // 建造部队
        view.MakeTroop(troopType, view.curTileX, view.curTileY);

        modelGame.MakeFinishAction(view.curTileX, view.curTileY);
    }
}
