using System;
using System.Collections;
using UnityEngine;


public class BaseA : MonoBehaviour
{
    public TextMesh txtIncome;
    public GameObject flagMake;
    public GameObject incomeObj;


    private void Start()
    {
        onOccupyChanged();

        incomeObj.SetActive(false);
        flagMake.SetActive(true);

        OccupyChangeSignal signal =(GameRoot.Instance.context as GameContext).injectionBinder.GetInstance<OccupyChangeSignal>();
        signal.AddListener(onOccupyChanged);

        MapRefreshSignal signal2 = (GameRoot.Instance.context as GameContext).injectionBinder.GetInstance<MapRefreshSignal>();
        signal2.AddListener(onPlayerTurnStart);
    }

    private void onPlayerTurnStart()
    {
        incomeObj.SetActive(true);
        flagMake.SetActive(false);

        StartCoroutine(showIncome());
    }

    private IEnumerator showIncome()
    {
        yield return new WaitForSeconds(1.5f);

        incomeObj.SetActive(false);

        yield return new WaitForSeconds(0.2f);
        flagMake.SetActive(true);
    }

    private void onOccupyChanged()
    {
        GetOccupyTileSignal signal = (GameRoot.Instance.context as GameContext).injectionBinder.GetInstance<GetOccupyTileSignal>();
        signal.Dispatch(eCountry.A);

        txtIncome.text = "+ " + (signal.OccupyTileCount * GameDef.PER_TILE_INCOME);
    }
}
