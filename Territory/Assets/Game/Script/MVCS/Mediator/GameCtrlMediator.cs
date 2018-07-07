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
    public PlayerModel modelPlayer { get; set; }
    [Inject]
    public GameModel modelGame { get; set; }
    [Inject]
    public ShowNofitySignal signalShowNotify { get; set; }


    override public void OnRegister()
    {
        signalFrame.AddListener(onFrame);
        signalEndTurn.AddListener(onEndTurn);

        modelPlayer.COIN = 40;

        signalStartup.Dispatch();
    }

    private void onEndTurn(bool aCountry)
    {
        modelGame.gameStatus = eInGameStatus.Trans;

        if(aCountry)
        {
            signalShowNotify.Dispatch("我军回合结束");
            StartCoroutine(delayChangeStatus(eInGameStatus.BTurn));
        }
        else
        {
            signalShowNotify.Dispatch("敌军回合结束");
            StartCoroutine(delayChangeStatus(eInGameStatus.ATurn));
        }
    }

    private IEnumerator delayChangeStatus(eInGameStatus status)
    {
        yield return new WaitForSeconds(1.0f);

        modelGame.gameStatus = status;
    }


    private bool bTurnFlag = false;

    private void onFrame()
    {
        //TODO 

        ///////////////////////////////////////////////////////[TEMP]
        if (modelGame.gameStatus == eInGameStatus.BTurn && !bTurnFlag)
        {
            bTurnFlag = true;
            StartCoroutine(___endBTurn());
        }
        ///////////////////////////////////////////
    }

    private IEnumerator ___endBTurn()
    {
        yield return new WaitForSeconds(1.0f);
        signalEndTurn.Dispatch(false);
        bTurnFlag = false;
    }
}
