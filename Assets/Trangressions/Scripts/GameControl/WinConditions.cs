using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditions : MonoBehaviour {

    LevelTimer lTime;

    public bool targetKilled;
    public bool reachedExit;
    public bool inTime;

    public static bool allWinConditionsMet;

    GameObject[] enemyGameObjects;
    Enemy[] allEnemies;
    GameObject exitPoint;

    private void Awake()
    {
        if (exitPoint == null)
            exitPoint = GameObject.FindGameObjectWithTag("ExitPoint");
        if (exitPoint.activeInHierarchy)
            exitPoint.SetActive(false);

        //RestartWinConditions();
        //ResetConditions();
        //print("win condition awake");
    }

    private void Start()
    {
        //print("WinCondition start");
        RestartWinConditions();
    }

    public void RestartWinConditions()
    {
        lTime = GetComponent<LevelTimer>();

        enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        allEnemies = new Enemy[enemyGameObjects.Length];

        for (int i = 0; i < enemyGameObjects.Length; i++)
        {
            allEnemies[i] = enemyGameObjects[i].GetComponent<Enemy>();
        }

        if (exitPoint == null)
        {
            exitPoint = GameObject.FindGameObjectWithTag("ExitPoint");
        }

        print("Restart winConditions");
    }

    private void Update()
    {
        if (!allWinConditionsMet && !PauseMenu.isPaused)
        {
            for (int i = 0; i < allEnemies.Length; i++)
            {
                if (allEnemies[i].isTarget && allEnemies[i].dead)
                {
                    targetKilled = true;
                    ActivateExitPoint();
                    //print("Target is dead");
                }
            }

            if (lTime.currentTime > 0)
            {
                inTime = true;
            }

            if (targetKilled && reachedExit && inTime)
            {
                allWinConditionsMet = true;
                GameController.gc.PlayerWin();
               // ResetConditions();
            }
        }
        if (exitPoint == null)
        {
            exitPoint = GameObject.FindGameObjectWithTag("ExitPoint");
            
        }

    }

    void ActivateExitPoint()
    {
        //exitPoint.SetActive(true);
    }

    public void ResetConditions()
    {
        targetKilled = false;
        reachedExit = false;
        inTime = false;

        for (int i = 0; i < allEnemies.Length; i++)
        {
            allEnemies[i].ResetEnemy();
        }

        print("Reset conditions " + targetKilled);
    }

    private void OnDestroy()
    {
        //print("Winconditions disabled");
       // ResetConditions();
    }
}
