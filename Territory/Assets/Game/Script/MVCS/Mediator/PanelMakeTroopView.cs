using UnityEngine;
using UnityEngine.UI;


public class PanelMakeTroopView : BaseView
{
    public Text txtCurCoin;
    public Text txtPriceTroop1;
    public Text txtPriceTroop2;
    public Text txtPriceTroop3;

    public Toggle troop1;
    public Toggle troop2;
    public Toggle troop3;

    [Inject]
    public PlayerModel modelPlayer { get; set; }
    [Inject]
    public GameModel modelGame { get; set; }

    [Inject]
    public MakeTroopSignal signalMakeTroop { get; set; }


    public void onSelectChange()
    {
        //TODO 
    }

    /// <summary>
    /// 确认建造
    /// </summary>
    public void onConfirm()
    {
        Debug.Log("PanelMakeTroopView: onConfirm");

        if(troop1.isOn)
        {
            int price = modelGame.GetTroopPrice(eTroopType.scissors);

            if (modelPlayer.COIN >= price)
            {
                // 扣钱
                modelPlayer.COIN = modelPlayer.COIN - price;

                signalMakeTroop.Dispatch(eTroopType.scissors, eCountry.A, MapMediator.curTileX, MapMediator.curTileY);
                Debug.Log("make troop1");

                onClose();
            }
        }
        else if(troop2.isOn)
        {
            int price = modelGame.GetTroopPrice(eTroopType.rock);

            if (modelPlayer.COIN >= price)
            {
                // 扣钱
                modelPlayer.COIN = modelPlayer.COIN - price;

                signalMakeTroop.Dispatch(eTroopType.rock, eCountry.A, MapMediator.curTileX, MapMediator.curTileY);
                Debug.Log("make troop2");

                onClose();
            }
        }
        else if(troop3.isOn)
        {
            int price = modelGame.GetTroopPrice(eTroopType.paper);

            if (modelPlayer.COIN >= price)
            {
                // 扣钱
                modelPlayer.COIN = modelPlayer.COIN - price;

                signalMakeTroop.Dispatch(eTroopType.paper, eCountry.A, MapMediator.curTileX, MapMediator.curTileY);
                Debug.Log("make troop3");

                onClose();
            }
        }
    }
}
