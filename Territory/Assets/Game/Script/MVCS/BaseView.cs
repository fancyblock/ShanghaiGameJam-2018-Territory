using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseView : View
{

	public void onClose()
    {
        gameObject.SetActive(false);
        (GameRoot.Instance.context as GameContext).injectionBinder.GetInstance<GameModel>().UI_POPUP = false;
    }
}
