using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditions : MonoBehaviour {

    public bool targetKilled;
    public bool reachedExit;
    public bool inTime;

    public static WinConditions wc;

    private void Awake()
    {
        if (wc == null)
        {
            wc = this;
        }
    }
}
