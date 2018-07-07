using strange.extensions.mediation.impl;


public class PanelMakeTroopMediator : Mediator
{
    [Inject]
    public PanelMakeTroopView m_view { get; set; }
    [Inject]
    public PlayerModel modelPlayer { get; set; }
    [Inject]
    public PopupUISignal signalPopupUI { get; set; }
    [Inject]
    public GameModel modelGame { get; set; }


    override public void OnRegister()
    {
        gameObject.SetActive(false);

        signalPopupUI.AddListener(onPopup);
    }


    private void onPopup(eUI ui)
    {
        if (ui == eUI.MakeTroop)
        {
            m_view.txtCurCoin.text = "" + modelPlayer.COIN;
            m_view.txtPriceTroop1.text = "" + modelGame.GetTroopPrice(eTroopType.scissors);
            m_view.txtPriceTroop2.text = "" + modelGame.GetTroopPrice(eTroopType.rock);
            m_view.txtPriceTroop3.text = "" + modelGame.GetTroopPrice(eTroopType.paper);

            gameObject.SetActive(true);
            modelGame.UI_POPUP = true;
        }
    }
}
