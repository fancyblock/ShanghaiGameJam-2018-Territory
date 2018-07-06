using strange.extensions.mediation.impl;


public class GameCtrlMediator : Mediator
{
    [Inject]
    public FrameSignal m_signalFrame { get; set; }
    [Inject]
    public StartupSignal m_signalStartup { get; set; }


    override public void OnRegister()
    {
        m_signalFrame.AddListener(onFrame);

        m_signalStartup.Dispatch();
    }


    private void onFrame()
    {
        //TODO 
    }
}
