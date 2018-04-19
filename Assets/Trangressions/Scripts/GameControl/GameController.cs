using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController gc;

    Player player;
    GameObject playerObj;

    private void Awake()
    {
        if (!gc)
        {
            gc = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = GetComponent<Player>();
    }

    public bool PlayerLost()
    {
        if (LevelTimer.lTime.currentTime <= 0 || !player.alive)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PlayerWin()
    {
        return false;
    }
}
