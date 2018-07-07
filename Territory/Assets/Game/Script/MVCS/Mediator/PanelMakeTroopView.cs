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
        if (modelPlayer.COIN >= modelGame.GetTroopPrice(eTroopType.scissors))
        {
            signalMakeTroop.Dispatch(eTroopType.scissors);
            Debug.Log("make troop1");

            onClose();
        }
    }

    public void onMakeTroop2()
    {
        if (modelPlayer.COIN >= modelGame.GetTroopPrice(eTroopType.rock))
        {
            signalMakeTroop.Dispatch(eTroopType.rock);
            Debug.Log("make troop2");

            onClose();
        }
    }

    public void onMakeTroop3()
    {
        if (modelPlayer.COIN >= modelGame.GetTroopPrice(eTroopType.paper))
        {
            signalMakeTroop.Dispatch(eTroopType.paper);
            Debug.Log("make troop3");

            onClose();
        }
    }
}
