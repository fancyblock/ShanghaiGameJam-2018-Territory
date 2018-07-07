using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseA : MonoBehaviour
{
    public TextMesh txtIncome;


    private void Start()
    {
        onOccupyChanged();

        OccupyChangeSignal signal =(GameRoot.Instance.context as GameContext).injectionBinder.GetInstance<OccupyChangeSignal>();
        signal.AddListener(onOccupyChanged);
    }

    private void onOccupyChanged()
    {
        GetOccupyTileSignal signal = (GameRoot.Instance.context as GameContext).injectionBinder.GetInstance<GetOccupyTileSignal>();
        signal.Dispatch(eCountry.A);

        txtIncome.text = "+ " + (signal.OccupyTileCount * GameDef.PER_TILE_INCOME);
    }
}
