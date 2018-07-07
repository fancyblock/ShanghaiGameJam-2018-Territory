using System;
using UnityEngine;


public class MapTile : MonoBehaviour
{
    private int x, y;
    private Action<int, int> callback;


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
