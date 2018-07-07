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

    private int curTileX, curTileY;
    private Troop curTroop;

    private bool noOperate = false;


    override public void OnRegister()
    {
        signalStartup.AddListener(onGameStart);
        signalMakeTroop.AddListener(onMakeTroop);
        signalMapRefresh.AddListener(onMapRefresh);
    }

    private void onMapRefresh()
    {
        // 部队的行动力恢复
        foreach(var tile in view.mapTiles.Values)
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
        if (noOperate || modelGame.UI_POPUP)
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
                        //TODO 
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
        if (noOperate || modelGame.UI_POPUP)
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


    // 移动部队
    private void moveTroop(Troop troop, int x, int y)
    {
        StartCoroutine(movingTroop(troop, x, y));
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

        for(int i = 0; i < view.mapData.width; i++)
        {
            for(int j = 0; j < view.mapData.height; j++)
            {
                MapTile mt = view.GetTile(i, j);

                if(mt != null && mt.type == eTileType.CrossLand)
                {
                    if(mt.troop)
                    {
                        if (mt.country != mt.troop.country)
                            tile = mt;
                    }
                }
            }
        }

        if (tile)
            StartCoroutine(occupyingTile(tile));
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
    }

    private void cancelSelectTroop(Troop troop)
    {
        view.CloseGridHint();
        troop.ShowOutline(false);

        curTroop = null;
    }

    // 建造单位
    private void onMakeTroop(eTroopType troopType)
    {
        Debug.Log("make troop: " + troopType.ToString());

        int price = modelGame.GetTroopPrice(troopType);

        // 扣钱
        modelPlayer.COIN = modelPlayer.COIN - price;
        // 建造部队
        view.MakeTroop(troopType, curTileX, curTileY);
    }
}
