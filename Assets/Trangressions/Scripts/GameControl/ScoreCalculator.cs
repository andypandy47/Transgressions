using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour {
    LevelTimer timer;

    [HideInInspector] public int nbKilled, killScore;
    public int levelCompleteScore;
    public int AGradeScore;
    public int BGradeScore;
    public int CGradeScore;
    public int DGradeScore;
    
    int finalScore;

    private void Start()
    {
        timer = GetComponent<LevelTimer>();
    }

    public int CalculateTimeScore()
    {
        int timeScore = Mathf.RoundToInt(timer.currentTime * 1000);

        return timeScore;
    }

    public void AddEnemyToScore(Enemy.EnemyType eType)
    {
        if (eType == Enemy.EnemyType.NonBeleiver)
        {
            killScore += 250;
        }
    }

    public string CalculateRank()
    {
        finalScore = levelCompleteScore + CalculateTimeScore() + killScore;

        if (finalScore >= AGradeScore)
        {
            return ("A");
        }
        else if (finalScore >= BGradeScore)
        {
            return ("B");
        }
        else if (finalScore >= CGradeScore)
        {
            return ("C");
        }
        else if (finalScore >= DGradeScore)
        {
            return ("D");
        }
        else
            return null;
    }

    public void ResetScore()
    {
        killScore = 0;
    }
}
