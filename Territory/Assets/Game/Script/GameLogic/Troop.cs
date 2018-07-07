using UnityEngine;
using UnityEngine.Rendering;
using System;


public class Troop : MonoBehaviour
{
    public eTroopType type;
    public eCountry country;
    public SortingGroup sortingGroup;
    public int x;
    public int y;

    public GameObject outline;
    public GameObject flagFinishAct;

    private bool finishAct = false;
    private Action<Troop> callback;


    void Awake()
    {
        if (outline)
            outline.SetActive(false);
    }

    public void SetTapCallback(Action<Troop> callback)
    {
        this.callback = callback;
    }

    public void ShowOutline(bool show)
    {
        if(outline)
            outline.SetActive(show);
    }


    /// <summary>
    /// 被点击
    /// </summary>
    void OnMouseDown()
    {
        if (callback != null)
            callback(this);
    }

    public bool FINISH_ACTION
    {
        set
        {
            flagFinishAct.SetActive(value);
            finishAct = value;
        }
        get
        {
            return finishAct;
        }
    }
}
