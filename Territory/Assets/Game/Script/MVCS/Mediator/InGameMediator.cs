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
    [Inject]
    public PlayerModel modelPlayer { get; set; }
    [Inject]
    public ShowNofitySignal signalShowNotify { get; set; }


    override public void OnRegister()
    {
        signalFrame.AddListener(onFrame);
        signalShowNotify.AddListener(onShowNotify);
    }

    private void onShowNotify(string obj, bool red)
    {
        view.ShowNotify(obj, red);
    }

    private void onFrame()
    {
        view.txtCoin.text = modelPlayer.COIN.ToString();

        if (modelGame.gameStatus == eInGameStatus.ATurn)
        {
            view.ATurnBg.SetActive(true);
            view.BTurnBg.SetActive(false);

            view.txtTitle.text = "我军回合";
        }
        else if(modelGame.gameStatus == eInGameStatus.BTurn)
        {
            view.ATurnBg.SetActive(false);
            view.BTurnBg.SetActive(true);

            view.txtTitle.text = "敌军回合";
        }
        else if(modelGame.gameStatus == eInGameStatus.Trans)
        {
            view.ATurnBg.SetActive(false);
            view.BTurnBg.SetActive(false);

            view.txtTitle.text = "攻守交换";
        }
    }
}
