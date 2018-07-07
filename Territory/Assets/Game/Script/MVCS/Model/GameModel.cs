using System.Collections.Generic;


public class GameModel
{
    public eInGameStatus gameStatus { get; set; }

    private HashSet<string> finishActTiles = new HashSet<string>();


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

    public bool IsFinishAction(int x, int y)
    {
        return finishActTiles.Contains(x + "_" + y);
    }

    public void MakeFinishAction(int x, int y)
    {
        finishActTiles.Add(x + "_" + y);
    }
}
