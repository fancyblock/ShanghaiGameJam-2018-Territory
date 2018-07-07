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
        mediationBinder.Bind<AIView>().To<AIMediator>();
        mediationBinder.Bind<InfoView>().To<InfoMediator>();
        mediationBinder.Bind<GameOverView>().To<GameOverMediator>();
        mediationBinder.Bind<TitleView>().To<TitleMediator>();

        injectionBinder.Bind<StartupSignal>().ToSingleton();
        injectionBinder.Bind<FrameSignal>().ToSingleton();
        injectionBinder.Bind<PopupUISignal>().ToSingleton();
        injectionBinder.Bind<MakeTroopSignal>().ToSingleton();
        injectionBinder.Bind<EndTurnSignal>().ToSingleton();
        injectionBinder.Bind<ShowNofitySignal>().ToSingleton();
        injectionBinder.Bind<GameStatusChangeSignal>().ToSingleton();
        injectionBinder.Bind<MapRefreshSignal>().ToSingleton();
        injectionBinder.Bind<GetOccupyTileSignal>().ToSingleton();
        injectionBinder.Bind<OccupyChangeSignal>().ToSingleton();
        injectionBinder.Bind<MoveTroopSignal>().ToSingleton();
        injectionBinder.Bind<GameOverSignal>().ToSingleton();
    }

    protected override void addCoreComponents()
    {
        base.addCoreComponents();

        // 使用Signal替代Command
        injectionBinder.Unbind<ICommandBinder>();
        injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
    }
}
