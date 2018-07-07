using System;
using UnityEngine;
using UnityEngine.Rendering;

public class MapTile : MonoBehaviour
{
    public GameObject fobiden;
    public GameObject move;
    public GameObject attack;

    private int x, y;
    private Action<int, int> callback;

    public Troop troop { get; set; }

    public void SetOrder(int order)
    {
        GetComponent<SortingGroup>().sortingOrder = order;
    }

    public void CloseHints()
    {
        fobiden.SetActive(false);
        move.SetActive(false);
        attack.SetActive(false);
    }

    public void ShowAttack()
    {
        fobiden.SetActive(false);
        move.SetActive(false);
        attack.SetActive(true);
    }

    public void ShowMove()
    {
        fobiden.SetActive(false);
        move.SetActive(true);
        attack.SetActive(false);
    }

    public void ShowGrey()
    {
        fobiden.SetActive(true);
        move.SetActive(false);
        attack.SetActive(false);
    }


    void OnMouseUpAsButton()
    {
        if (callback != null)
            callback(x, y);
    }

    public void SetTile(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetTapCallback(Action<int, int> callback)
    {
        this.callback = callback;
    }
}
