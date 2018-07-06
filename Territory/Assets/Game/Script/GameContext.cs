﻿using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.impl;
using UnityEngine;


public class GameContext : MVCSContext
{
    public GameContext(MonoBehaviour view) : base(view) { }


    protected override void mapBindings()
    {
        mediationBinder.Bind<GameCtrlView>().To<GameCtrlMediator>();

        injectionBinder.Bind<StartupSignal>().ToSingleton();
        injectionBinder.Bind<FrameSignal>().ToSingleton();
    }

    protected override void addCoreComponents()
    {
        base.addCoreComponents();

        // 使用Signal替代Command
        injectionBinder.Unbind<ICommandBinder>();
        injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
    }
}
