using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController gc;

    Player player;
    GameObject playerObj;
    GameObject UICanvas;

    PauseMenu pMenu;

    private void Awake()
    {
        if (!gc)
        {
            gc = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        UICanvas = GameObject.FindGameObjectWithTag("UICanvas");
    }

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = GetComponent<Player>();
        pMenu = UICanvas.GetComponent<PauseMenu>();
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

    public void PlayerWin()
    {
        WinConditions.allWinConditionsMet = false;
        pMenu.Pause(false);
    }
}
