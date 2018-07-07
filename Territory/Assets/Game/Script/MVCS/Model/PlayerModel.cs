using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerModel
{
    private int coin;


    public int COIN
    {
        get
        {
            return coin;
        }
        set
        {
            coin = value;

            // 通知钱变更
        }
    }

}
