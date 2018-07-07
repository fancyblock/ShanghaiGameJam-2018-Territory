using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class InGameView : BaseView
{
    public Text txtTitle;
    public GameObject notifyBg;
    public Text txtNotifyInfo;

    public GameObject ATurnBg;
    public GameObject BTurnBg;

    [Inject]
    public EndTurnSignal signalEndTurn { get; set; }
    [Inject]
    public GameModel modelGame { get; set; }


    // 手动结束回合
    public void EndTurn()
    {
        Debug.Log("end my turn");

        //TODO 

        modelGame.gameStatus = eInGameStatus.BTurn;

        signalEndTurn.Dispatch(true);

        ShowNotify("回合结束");
    }


    public void ShowNotify(string info)
    {
        txtNotifyInfo.text = info;

        StartCoroutine(showingNotify());
    }

    private IEnumerator showingNotify()
    {
        TweenPosition.Begin(notifyBg, 0.2f, Vector3.zero).from = new Vector2(-1200,0);

        yield return new WaitForSeconds(1.0f);

        TweenPosition.Begin(notifyBg, 0.2f, new Vector2(1200,0));
    }
}
