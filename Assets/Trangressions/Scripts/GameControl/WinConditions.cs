using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditions : MonoBehaviour {

    LevelTimer lTime;

    public bool targetKilled;
    public bool reachedExit;
    public bool inTime;
    public bool playerLost;

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
    }

    private void Start()
    {   
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

        print("Win conditions start");
    }

    private void Update()
    {
        if (exitPoint == null)
        {
            exitPoint = GameObject.FindGameObjectWithTag("ExitPoint");
            print("Exitpoint was null");
        }

        if (!allWinConditionsMet && !PauseMenu.isPaused)
        {
            if (!targetKilled)
            {
                for (int i = 0; i < allEnemies.Length; i++)
                {
                    if (allEnemies[i].isTarget && allEnemies[i].dead)
                    {
                        targetKilled = true;
                        print("Target is dead");
                        ActivateExitPoint();
                    }
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
            }
        }
    }

    void ActivateExitPoint()
    {
        exitPoint.SetActive(true);
        print("Activate the exitpoint");
    }

    public void ResetConditions()
    {
        targetKilled = false;
        reachedExit = false;
        inTime = false;
        playerLost = false;
        allWinConditionsMet = false;

        for (int i = 0; i < allEnemies.Length; i++)
        {
            allEnemies[i].ResetEnemy();
        }

        if (exitPoint.activeInHierarchy)
            exitPoint.SetActive(false);

        print("Reset wincondition variables ");
    }
 
}
