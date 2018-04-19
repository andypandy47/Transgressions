using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour {

    public static ScoreCalculator sCalc;

    LevelTimer timer;

    [HideInInspector] public int nbKilled;

    int score = 0;
    int finalScore;

    private void Awake()
    {
        if (sCalc == null)
            sCalc = this;
    }

    private void Start()
    {
        timer = GetComponent<LevelTimer>();
    }

    public int CalculateScore()
    {
        int timeScore = Mathf.RoundToInt(timer.currentTime);

        finalScore = timeScore + score;

        return finalScore;
    }

    public void AddEnemyToScore(Enemy.EnemyType eType)
    {
        if (eType == Enemy.EnemyType.NonBeleiver)
        {
            score += 100;
        }
    }
}
