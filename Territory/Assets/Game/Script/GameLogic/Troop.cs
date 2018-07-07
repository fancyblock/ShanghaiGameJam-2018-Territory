using UnityEngine;
using UnityEngine.Rendering;


public class Troop : MonoBehaviour
{
    public SortingGroup sortingGroup;

    public GameObject flagFinishAct;

    private bool finishAct = false;


	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {	
	}

    /// <summary>
    /// 被点击
    /// </summary>
    void OnMouseDown()
    {
        if (finishAct)
            return;

        //TODO 

        //////////////////
        FINISH_ACTION = true;
    }

    public bool FINISH_ACTION
    {
        set
        {
            flagFinishAct.SetActive(value);
            finishAct = value;
        }
    }
}
