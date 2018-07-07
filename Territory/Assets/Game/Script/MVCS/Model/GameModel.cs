using System.Collections.Generic;


public class GameModel
{
    public eInGameStatus gameStatus { get; set; }

    public bool UI_POPUP { get; set; }


    public int GetTroopPrice(eTroopType type)
    {
        switch (type)
        {
            case eTroopType.rock:
                return GameDef.BASE_TROOP_COST;
            case eTroopType.paper:
                return GameDef.BASE_TROOP_COST;
            case eTroopType.scissors:
                return GameDef.BASE_TROOP_COST;
            default:
                break;
        }

        return 0;
    }
}
