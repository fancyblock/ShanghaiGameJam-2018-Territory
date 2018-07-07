using strange.extensions.mediation.impl;


public class GameCtrlMediator : Mediator
{
    [Inject]
    public FrameSignal signalFrame { get; set; }
    [Inject]
    public StartupSignal signalStartup { get; set; }

    [Inject]
    public PlayerModel modelPlayer { get; set; }


    override public void OnRegister()
    {
        signalFrame.AddListener(onFrame);

        modelPlayer.COIN = 40;

        signalStartup.Dispatch();
    }


    private void onFrame()
    {
        //TODO 
    }
}
