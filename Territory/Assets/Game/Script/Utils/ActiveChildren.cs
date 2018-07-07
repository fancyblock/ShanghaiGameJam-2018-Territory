using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActiveChildren : MonoBehaviour
{
	// Use this for initialization
	void Awake ()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
	}
}
