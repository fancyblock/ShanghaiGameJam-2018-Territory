using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameModel
{
    public int GetTroopPrice(eTroopType type)
    {
        switch (type)
        {
            case eTroopType.rock:
                return 30;
                break;
            case eTroopType.paper:
                return 50;
                break;
            case eTroopType.scissors:
                return 70;
                break;
            default:
                break;
        }

        return 0;
    }
}
