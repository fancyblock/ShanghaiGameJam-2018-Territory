using System.Collections.Generic;


public class GameModel
{
    public eInGameStatus gameStatus { get; set; }

    public Dictionary<string, MapTile> mapTiles = new Dictionary<string, MapTile>();

    public bool UI_POPUP { get; set; }


    private int rockPriceOffset = 0;
    private int paperPriceOffset = 0;
    private int scissorPriceOffset = 0;


    public void RefreshPrice()
    {
        int rockCnt = 0;
        int paperCnt = 0;
        int scissorCnt = 0;

        foreach(MapTile mt in mapTiles.Values)
        {
            if(mt.troop)
            {
                switch (mt.troop.type)
                {
                    case eTroopType.rock:
                        rockCnt++;
                        break;
                    case eTroopType.paper:
                        paperCnt++;
                        break;
                    case eTroopType.scissors:
                        scissorCnt++;
                        break;
                    default:
                        break;
                }
            }
        }

        rockPriceOffset = (rockCnt - 1) * 5;
        paperPriceOffset = (paperCnt - 1) * 5;
        scissorPriceOffset = (scissorCnt - 1) * 5;
    }

    public int GetTroopPrice(eTroopType type)
    {
        switch (type)
        {
            case eTroopType.rock:
                return GameDef.BASE_TROOP_COST + rockPriceOffset;
            case eTroopType.paper:
                return GameDef.BASE_TROOP_COST + paperPriceOffset;
            case eTroopType.scissors:
                return GameDef.BASE_TROOP_COST + scissorPriceOffset;
            default:
                break;
        }

        return 0;
    }
}
