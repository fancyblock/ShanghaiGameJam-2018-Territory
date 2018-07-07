using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PanelMakeTroopMediator : Mediator
{
    [Inject]
    public PopupUISignal signalPopupUI { get; set; }


    override public void OnRegister()
    {
        gameObject.SetActive(false);

        signalPopupUI.AddListener(onPopup);
    }


    private void onPopup(eUI ui)
    {
        if (ui == eUI.MakeTroop)
        {
            //TODO 

            gameObject.SetActive(true);
        }
    }
}
