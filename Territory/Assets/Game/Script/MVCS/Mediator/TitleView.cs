using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleView : BaseView
{
    //TODO 

    public void onBegin()
    {
        gameObject.SetActive(false);

        StartupSignal signal = (GameRoot.Instance.context as GameContext).injectionBinder.GetInstance<StartupSignal>();
        signal.Dispatch();
    }
}
