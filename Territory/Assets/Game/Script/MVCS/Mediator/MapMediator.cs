using System;
using System.Collections;
using strange.extensions.mediation.impl;
using UnityEngine;


public class MapMediator : Mediator
{
    [Inject]
    public MapView view { get; set; }
    [Inject]
    public StartupSignal signalStartup { get; set; }
    [Inject]
    public MakeTroopSignal signalMakeTroop { get; set; }
    [Inject]
    public GameModel modelGame { get; set; }
    [Inject]
    public PlayerModel modelPlayer { get; set; }
    [Inject]
    public GameStatusChangeSignal signalGameStatusChange { get; set; }
    [Inject]
    public MapRefreshSignal signalMapRefresh { get; set; }
    [Inject]
    public PopupUISignal signalPopupUI { get; set; }
    [Inject]
    public GetOccupyTileSignal signalGetOccupyTile { get; set; }
    [Inject]
    public OccupyChangeSignal siganlOccupyChange { get; set; }
    [Inject]
    public MoveTroopSignal signalMoveTroop { get; set; }
    [Inject]
    public GameOverSignal signalGameOver { get; set; }

    private Troop curTroop;

    static public int curTileX, curTileY;
    static public bool noOperate = false;


    override public void OnRegister()
    {
        signalStartup.AddListener(onGameStart);
        signalMakeTroop.AddListener(onMakeTroop);
        signalMapRefresh.AddListener(onMapRefresh);
        signalGetOccupyTile.AddListener(onGetOccupyTile);
        signalMoveTroop.AddListener(moveTroop);
    }

    private void onMapRefresh()
    {
        // 部队的行动力恢复
        foreach(var tile in modelGame.mapTiles.Values)
        {
            if (tile.troop)
                tile.troop.FINISH_ACTION = false;
        }
    }

    private void onGameStart()
    {
        view.CreateMap();
    }

    public void onTapTile(int x, int y)
    {
        if (noOperate || modelGame.UI_POPUP || modelGame.gameStatus != eInGameStatus.ATurn)
            return;

        MapTile mt = view.GetTile(x, y);

        // 移动部队或进攻
        if (curTroop)
        {
            if(curTroop.x == x && curTroop.y == y)
            {
                cancelSelectTroop(curTroop);
            }
            else
            {
                if(Math.Abs(curTroop.x - x) + Math.Abs(curTroop.y - y) == 1)
                {
                    if(mt.troop)
                    {
                        // 攻击
                        moveTroop(curTroop, x, y);
                    }
                    else
                    {
                        // 移动到这一格
                        moveTroop(curTroop, x, y);
                    }
                }
            }
        }
        // 建造部队
        else if (mt.country == eCountry.A && (mt.type == eTileType.FactoryLand || mt.type == eTileType.CoreLand))
        {
            if (mt.troop)       // 上面有部队
            {
                onTapTroop(mt.troop);
                return;
            }

            curTileX = x;
            curTileY = y;

            signalPopupUI.Dispatch(eUI.MakeTroop);
        }
        else if(mt.troop)
        {
            onTapTroop(mt.troop);
        }
    }

    public void onTapTroop(Troop troop)
    {
        if(troop.country == eCountry.B && curTroop != null)
        {
            onTapTile(troop.x, troop.y);
        }

        if (troop.country != eCountry.A)
            return;

        if (noOperate || modelGame.UI_POPUP || modelGame.gameStatus != eInGameStatus.ATurn)
            return;

        if (troop.FINISH_ACTION)
            return;

        if (curTroop == troop)
        {
            cancelSelectTroop(troop);
            return;
        }

        if (curTroop != null)
            return;

        view.ShowGridHint(troop.x, troop.y);
        troop.ShowOutline(true);

        curTroop = troop;
    }

    private void onGetOccupyTile(eCountry country)
    {
        int i = 0;

        foreach(MapTile mt in modelGame.mapTiles.Values)
        {
            if (mt.country == country)
                i++;
        }

        signalGetOccupyTile.OccupyTileCount = i;
    }


    // 移动部队
    private void moveTroop(Troop troop, int x, int y)
    {
        MapTile destTile = view.GetTile(x, y);

        if (destTile.troop == null)
            StartCoroutine(movingTroop(troop, x, y));   // 普通移动
        else
            StartCoroutine(movingFight(troop, destTile.troop, x, y, destTile));   // 战斗
    }

    // 战斗
    private IEnumerator movingFight(Troop troop, Troop enimy, int x, int y, MapTile battleField)
    {
        noOperate = true;

        view.CloseGridHint();
        troop.ShowOutline(false);

        TweenPosition.Begin(troop.gameObject, 1.0f, view.GridToPosition(x, y));
        TweenScale ts = TweenScale.Begin(troop.gameObject, 0.25f, new Vector3(1.0f, 1.1f));
        ts.style = UITweener.Style.PingPong;

        yield return new WaitForSeconds(0.7f);

        view.PlayCloudAnim(x, y);

        yield return new WaitForSeconds(0.3f);
        Destroy(ts);

        yield return new WaitForSeconds(0.7f);

        view.GetTile(troop.x, troop.y).troop = null;
        Troop winTroop = getWinner(troop, enimy);

        if(winTroop == null)
        {
            // 同归于尽
            Destroy(troop.gameObject);
            Destroy(enimy.gameObject);
            battleField.troop = null;
        }
        else if(winTroop == troop)
        {
            // 进攻方赢
            Destroy(enimy.gameObject);

            troop.sortingGroup.sortingOrder = view.GetTroopOrder(x, y);

            battleField.troop = troop;

            troop.x = x;
            troop.y = y;

            troop.FINISH_ACTION = true;
        }
        else if(winTroop == enimy)
        {
            // 防守方赢
            Destroy(troop.gameObject);
        }

        yield return new WaitForSeconds(1.0f);

        curTroop = null;

        noOperate = false;

        checkOccupy();
    }

    // 检查游戏胜负
    private void checkGameWin()
    {
        int aTroopCnt = 0;
        int bTroopCnt = 0;

        foreach(MapTile mt in modelGame.mapTiles.Values)
        {
            if(mt.troop)
            {
                if (mt.troop.country == eCountry.A)
                    aTroopCnt++;
                else if (mt.troop.country == eCountry.B)
                    bTroopCnt++;
            }
        }

        if (aTroopCnt == 0 && bTroopCnt > 0)
        {
            signalGameOver.Dispatch(eCountry.B);

            noOperate = true;
        }
        else if(bTroopCnt == 0 && aTroopCnt > 0)
        {
            signalGameOver.Dispatch(eCountry.A);

            noOperate = true;
        }
    }

    // 判断胜负 
    private Troop getWinner(Troop t1, Troop t2)
    {
        if(t1.type == eTroopType.paper)
        {
            switch (t2.type)
            {
                case eTroopType.rock:
                    return t1;
                case eTroopType.paper:
                    return null;
                case eTroopType.scissors:
                    return t2;
                default:
                    break;
            }
        }
        else if(t1.type == eTroopType.rock)
        {
            switch (t2.type)
            {
                case eTroopType.rock:
                    return null;
                case eTroopType.paper:
                    return t2;
                case eTroopType.scissors:
                    return t1;
                default:
                    break;
            }
        }
        else if(t1.type == eTroopType.scissors)
        {
            switch (t2.type)
            {
                case eTroopType.rock:
                    return t2;
                case eTroopType.paper:
                    return t1;
                case eTroopType.scissors:
                    return null;
                default:
                    break;
            }
        }

        return null;
    }

    private IEnumerator movingTroop(Troop troop, int x, int y)
    {
        noOperate = true;

        view.CloseGridHint();
        troop.ShowOutline(false);

        TweenPosition.Begin(troop.gameObject, 1.0f, view.GridToPosition(x, y));
        TweenScale ts = TweenScale.Begin(troop.gameObject, 0.25f, new Vector3(1.0f, 1.1f));
        ts.style = UITweener.Style.PingPong;

        yield return new WaitForSeconds(1.0f);

        Destroy(ts);

        troop.sortingGroup.sortingOrder = view.GetTroopOrder(x, y);

        view.GetTile(troop.x, troop.y).troop = null;
        view.GetTile(x, y).troop = troop;

        troop.x = x;
        troop.y = y;

        troop.FINISH_ACTION = true;

        curTroop = null;

        noOperate = false;

        checkOccupy();
    }

    // 检查占领
    private void checkOccupy()
    {
        MapTile tile = null;

        foreach(MapTile mt in modelGame.mapTiles.Values)
        {
            if (mt.type == eTileType.CrossLand)
            {
                if (mt.troop)
                {
                    if (mt.country != mt.troop.country)
                        tile = mt;
                }
            }
        }

        if (tile)
            StartCoroutine(occupyingTile(tile));
        else
            checkGameWin();
    }

    private IEnumerator occupyingTile(MapTile tile)
    {
        noOperate = true;

        int x = tile.x;
        int y = tile.y;
        Troop troop = tile.troop;

        view.PlayCloudAnim(x, y);

        yield return new WaitForSeconds(1.0f);

        view.ChangeTile(x, y, troop.country);

        yield return new WaitForSeconds(1.0f);

        view.GetTile(x, y).troop = troop;

        noOperate = false;

        siganlOccupyChange.Dispatch();

        checkGameWin();
    }

    private void cancelSelectTroop(Troop troop)
    {
        view.CloseGridHint();
        troop.ShowOutline(false);

        curTroop = null;
    }

    // 建造部队
    private void onMakeTroop(eTroopType troopType, eCountry country, int x, int y)
    {
        view.MakeTroop(troopType, country, x, y);
    }
}
