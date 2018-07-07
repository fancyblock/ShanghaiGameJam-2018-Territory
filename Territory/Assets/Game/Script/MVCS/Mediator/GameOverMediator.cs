using strange.extensions.mediation.impl;


public class GameOverMediator : Mediator
{
    [Inject]
    public GameOverView view { get; set; }
    [Inject]
    public GameOverSignal signalGameOver { get; set; }


    override public void OnRegister()
    {
        gameObject.SetActive(false);

        signalGameOver.AddListener(onGameOver);
    }


    private void onGameOver(eCountry winner)
    {
        if(winner == eCountry.A)
        {
            view.flagWin.SetActive(true);
            view.flagFail.SetActive(false);
        }
        else if(winner == eCountry.B)
        {
            view.flagWin.SetActive(false);
            view.flagFail.SetActive(true);
        }

        gameObject.SetActive(true);
    }
}
