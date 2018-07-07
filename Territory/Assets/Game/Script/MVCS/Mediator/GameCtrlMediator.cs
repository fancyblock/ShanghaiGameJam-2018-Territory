using System;
using System.Collections;
using strange.extensions.mediation.impl;
using UnityEngine;


public class GameCtrlMediator : Mediator
{
    [Inject]
    public FrameSignal signalFrame { get; set; }
    [Inject]
    public StartupSignal signalStartup { get; set; }
    [Inject]
    public EndTurnSignal signalEndTurn { get; set; }
    [Inject]
    public GameStatusChangeSignal signalGameStatusChange { get; set; }

    [Inject]
    public PlayerModel modelPlayer { get; set; }
    [Inject]
    public GameModel modelGame { get; set; }
    [Inject]
    public ShowNofitySignal signalShowNotify { get; set; }
    [Inject]
    public MapRefreshSignal signalMapRefresh { get; set; }
    [Inject]
    public GetOccupyTileSignal signalGetOccupyTile { get; set; }


    override public void OnRegister()
    {
        signalFrame.AddListener(onFrame);
        signalEndTurn.AddListener(onEndTurn);
        signalGameStatusChange.AddListener(onGameStatusChange);

        modelPlayer.COIN = GameDef.INIT_GOLD;

        //signalStartup.Dispatch();
    }

    /// <summary>
    /// 游戏状态改变
    /// </summary>
    /// <param name="status"></param>
    private void onGameStatusChange(eInGameStatus status)
    {
        switch (status)
        {
            case eInGameStatus.ATurn:
                signalMapRefresh.Dispatch();
                // 结算收入
                refreshIncome();
                break;
            case eInGameStatus.BTurn:
                //TODO 
                break;
            case eInGameStatus.Trans:
                break;
            default:
                break;
        }
    }

    private void onEndTurn(bool aCountry)
    {
        modelGame.gameStatus = eInGameStatus.Trans;
        signalGameStatusChange.Dispatch(modelGame.gameStatus);

        if (aCountry)
        {
            signalShowNotify.Dispatch("敌军回合开始", true);
            StartCoroutine(delayChangeStatus(eInGameStatus.BTurn));
        }
        else
        {
            signalShowNotify.Dispatch("我军回合开始", false);
            StartCoroutine(delayChangeStatus(eInGameStatus.ATurn));
        }
    }

    private IEnumerator delayChangeStatus(eInGameStatus status)
    {
        yield return new WaitForSeconds(1.0f);

        modelGame.gameStatus = status;
        signalGameStatusChange.Dispatch(modelGame.gameStatus);
    }

    // 结算我军收入 
    private void refreshIncome()
    {
        signalGetOccupyTile.Dispatch(eCountry.A);

        modelPlayer.COIN = modelPlayer.COIN + signalGetOccupyTile.OccupyTileCount * GameDef.PER_TILE_INCOME;
    }

    private void onFrame()
    {
        //TODO 
    }

}
