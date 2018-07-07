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
