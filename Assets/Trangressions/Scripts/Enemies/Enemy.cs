using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    BoxCollider2D collider;

    public enum EnemyType
    {
        NonBeleiver
    }
    public EnemyType eType;

    public float health;
    [HideInInspector]public Transform bloodPos;

    [HideInInspector] public bool dead;
    public bool isTarget;

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        bloodPos = transform.GetChild(0).transform;
    }

    public void DamageEnemy(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            KillEnemy();
        }
    }

    public void KillEnemy()
    {
        dead = true;
        collider.enabled = false;
        ScoreCalculator.sCalc.AddEnemyToScore(eType);
        LevelTimer.lTime.IncreaseTime(1);
    }
}
