using strange.extensions.mediation.impl;
using System.Collections;
using UnityEngine;


public class AIMediator : Mediator
{
    [Inject]
    public FrameSignal signalFrame { get; set; }
    [Inject]
    public GameStatusChangeSignal signalGameStatusChange { get; set; }
    [Inject]
    public EndTurnSignal signalEndTurn { get; set; }


    override public void OnRegister()
    {
        signalFrame.AddListener(onFrame);
        signalGameStatusChange.AddListener(onGameStatusChange);
    }

    private void onGameStatusChange(eInGameStatus status)
    {
        if (status == eInGameStatus.BTurn)
            StartCoroutine(___endBTurn());
    }

    private void onFrame()
    {
        //TODO 
    }


    private IEnumerator ___endBTurn()
    {
        yield return new WaitForSeconds(1.0f);

        signalEndTurn.Dispatch(false);
    }
}
