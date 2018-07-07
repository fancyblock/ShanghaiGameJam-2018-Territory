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

        foreach(MapTile mt in modelGame.mapTiles.Values)
        {
            if (mt.troop)
                troops.Add(mt.troop);
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

            //TODO 
        }

        yield return new WaitForSeconds(0.5f);

        signalEndTurn.Dispatch(false);
    }


    //TODO 
}
