using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.impl;
using UnityEngine;


public class GameContext : MVCSContext
{
    public GameContext(MonoBehaviour view) : base(view) { }


    protected override void mapBindings()
    {
        injectionBinder.Bind<PlayerModel>().ToSingleton();
        injectionBinder.Bind<GameModel>().ToSingleton();

        mediationBinder.Bind<GameCtrlView>().To<GameCtrlMediator>();
        mediationBinder.Bind<MapView>().To<MapMediator>();
        mediationBinder.Bind<PanelMakeTroopView>().To<PanelMakeTroopMediator>();
        mediationBinder.Bind<InGameView>().To<InGameMediator>();

        injectionBinder.Bind<StartupSignal>().ToSingleton();
        injectionBinder.Bind<FrameSignal>().ToSingleton();
        injectionBinder.Bind<PopupUISignal>().ToSingleton();
        injectionBinder.Bind<MakeTroopSignal>().ToSingleton();
        injectionBinder.Bind<EndTurnSignal>().ToSingleton();
        injectionBinder.Bind<ShowNofitySignal>().ToSingleton();
        injectionBinder.Bind<GameStatusChangeSignal>().ToSingleton();
        injectionBinder.Bind<MapRefreshSignal>().ToSingleton();
    }

    protected override void addCoreComponents()
    {
        base.addCoreComponents();

        // 使用Signal替代Command
        injectionBinder.Unbind<ICommandBinder>();
        injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
    }
}
