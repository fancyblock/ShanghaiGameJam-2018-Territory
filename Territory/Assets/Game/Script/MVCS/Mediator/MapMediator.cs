using System;
using strange.extensions.mediation.impl;


public class MapMediator : Mediator
{
    [Inject]
    public MapView view { get; set; }
    [Inject]
    public StartupSignal signalStartup { get; set; }
    [Inject]
    public MakeTroopSignal signalMakeTroop { get; set; }


    override public void OnRegister()
    {
        signalStartup.AddListener(onGameStart);
        signalMakeTroop.AddListener(onMakeTroop);
    }


    private void onGameStart()
    {
        view.CreateMap();
    }

    private void onMakeTroop(eTroopType troopType)
    {
        //TODO 
    }
}
