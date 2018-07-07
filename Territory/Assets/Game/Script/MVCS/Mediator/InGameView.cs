using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class InGameView : BaseView
{
    public Text txtTitle;
    public GameObject notifyMask;
    public GameObject notifyBg;
    public Image imgBg;
    public Text txtNotifyInfo;
    public Text txtCoin;

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

        signalEndTurn.Dispatch(true);
    }


    public void ShowNotify(string info, bool red)
    {
        txtNotifyInfo.text = info;

        if (red)
            imgBg.color = new Color(191f/255f, 88f/255f, 71f/255f);
        else
            imgBg.color = new Color(72f/255f, 173f/255f, 58f/255f);

        StartCoroutine(showingNotify());
    }

    private IEnumerator showingNotify()
    {
        notifyMask.SetActive(true);

        notifyBg.SetActive(true);
        TweenScale.Begin(notifyBg, 0.17f, Vector3.one).from = new Vector2(1,0);

        yield return new WaitForSeconds(1.3f);

        TweenScale.Begin(notifyBg, 0.17f, new Vector2(1,0));

        yield return new WaitForSeconds(0.17f);
        notifyBg.SetActive(false);

        notifyMask.SetActive(false);
    }
}
