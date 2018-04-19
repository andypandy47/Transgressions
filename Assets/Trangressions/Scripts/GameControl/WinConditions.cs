using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditions : MonoBehaviour {

    public bool targetKilled;
    public bool reachedExit;
    public bool inTime;

    public static WinConditions wc;
    public static bool allWinConditionsMet;

    Enemy[] allEnemies;
    GameObject exitPoint;

    private void Awake()
    {
        if (wc == null)
        {
            wc = this;
        }
    }

    private void Start()
    {
        GameObject[] enemyGameObjects;
        enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        allEnemies = new Enemy[enemyGameObjects.Length];

        for (int i = 0; i < enemyGameObjects.Length; i++)
        {
            allEnemies[i] = enemyGameObjects[i].GetComponent<Enemy>();
        }

        exitPoint = GameObject.FindGameObjectWithTag("ExitPoint");
        exitPoint.SetActive(false);
    }

    private void Update()
    {
        if (!allWinConditionsMet)
        {
            for (int i = 0; i < allEnemies.Length; i++)
            {
                if (allEnemies[i].isTarget && allEnemies[i].dead)
                {
                    targetKilled = true;
                    ActivateExitPoint();
                }
            }

            if (LevelTimer.lTime.currentTime > 0)
            {
                inTime = true;
            }

            if (targetKilled && reachedExit && inTime)
            {
                allWinConditionsMet = true;
                GameController.gc.PlayerWin();
                ResetConditions();
            }
        }
    }

    void ActivateExitPoint()
    {
        exitPoint.SetActive(true);
    }

    void ResetConditions()
    {
        targetKilled = false;
        reachedExit = false;
        inTime = false;
    }
}
