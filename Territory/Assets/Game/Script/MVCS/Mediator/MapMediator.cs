using System;
using strange.extensions.mediation.impl;


public class MapMediator : Mediator
{
    [Inject]
    public MapView mapView { get; set; }
    [Inject]
    public StartupSignal m_signalStartup { get; set; }


    override public void OnRegister()
    {
        m_signalStartup.AddListener(onGameStart);
    }


    private void onGameStart()
    {
        mapView.CreateMap();
    }
}
