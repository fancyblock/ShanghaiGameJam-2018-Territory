using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InGameView : BaseView
{
    public GameObject notifyBg;
    public Text txtNotifyInfo;

    [Inject]
    public EndTurnSignal signalEndTurn { get; set; }


    public void EndTurn()
    {
        Debug.Log("end my turn");

        //TODO 

        signalEndTurn.Dispatch(true);
    }
}
