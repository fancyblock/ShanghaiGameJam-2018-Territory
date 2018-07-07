using System;
using strange.extensions.mediation.impl;


public class InGameMediator : Mediator
{
    [Inject]
    public InGameView view { get; set; }
    [Inject]
    public FrameSignal signalFrame { get; set; }
    [Inject]
    public GameModel modelGame { get; set; }


    override public void OnRegister()
    {
        signalFrame.AddListener(onFrame);
    }


    private void onFrame()
    {
        if(modelGame.gameStatus == eInGameStatus.ATurn)
        {
            view.ATurnBg.SetActive(true);
            view.BTurnBg.SetActive(false);

            view.txtTitle.text = "我军回合";
        }
        else if(modelGame.gameStatus == eInGameStatus.BTurn)
        {
            view.ATurnBg.SetActive(false);
            view.BTurnBg.SetActive(true);

            //view.txtTitle
        }
    }
}
