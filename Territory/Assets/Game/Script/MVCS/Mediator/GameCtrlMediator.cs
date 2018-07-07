using strange.extensions.mediation.impl;


public class GameCtrlMediator : Mediator
{
    [Inject]
    public FrameSignal signalFrame { get; set; }
    [Inject]
    public StartupSignal signalStartup { get; set; }


    override public void OnRegister()
    {
        signalFrame.AddListener(onFrame);

        signalStartup.Dispatch();
    }


    private void onFrame()
    {
        //TODO 
    }
}
