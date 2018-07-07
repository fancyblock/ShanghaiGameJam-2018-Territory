using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverView : BaseView
{
    public GameObject flagWin;
    public GameObject flagFail;

	
	// Update is called once per frame
	public void onHome()
    {
        Debug.Log("onHome");

        SceneManager.LoadScene("Game");
    }

    public void onBack()
    {
        Debug.Log("onBack");

        //TODO 
    }
}
