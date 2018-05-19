using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPoint : MonoBehaviour {

    WinConditions wc;

    private void Start()
    {
        wc = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<WinConditions>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCollider")
        {
            print("Exit point reached");
            wc.reachedExit = true;
        }
    }
}
