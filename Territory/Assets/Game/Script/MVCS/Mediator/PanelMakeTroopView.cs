using UnityEngine;
using UnityEngine.UI;


public class PanelMakeTroopView : BaseView
{
    public Text txtCurCoin;
    public Text txtPriceTroop1;
    public Text txtPriceTroop2;
    public Text txtPriceTroop3;

    [Inject]
    public PlayerModel modelPlayer { get; set; }
    [Inject]
    public GameModel modelGame { get; set; }

    [Inject]
    public MakeTroopSignal signalMakeTroop { get; set; }


    public void onMakeTroop1()
    {
        int price = modelGame.GetTroopPrice(eTroopType.scissors);

        if (modelPlayer.COIN >= price)
        {
            // 扣钱
            modelPlayer.COIN = modelPlayer.COIN - price;

            signalMakeTroop.Dispatch(eTroopType.scissors, eCountry.A);
            Debug.Log("make troop1");

            onClose();
        }
    }

    public void onMakeTroop2()
    {
        int price = modelGame.GetTroopPrice(eTroopType.rock);

        if (modelPlayer.COIN >= price)
        {
            // 扣钱
            modelPlayer.COIN = modelPlayer.COIN - price;

            signalMakeTroop.Dispatch(eTroopType.rock, eCountry.A);
            Debug.Log("make troop2");

            onClose();
        }
    }

    public void onMakeTroop3()
    {
        int price = modelGame.GetTroopPrice(eTroopType.paper);

        if (modelPlayer.COIN >= modelGame.GetTroopPrice(eTroopType.paper))
        {
            // 扣钱
            modelPlayer.COIN = modelPlayer.COIN - price;

            signalMakeTroop.Dispatch(eTroopType.paper, eCountry.A);
            Debug.Log("make troop3");

            onClose();
        }
    }
}
