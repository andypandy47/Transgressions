using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    BoxCollider2D collider;
    LevelTimer lTime;

    public enum EnemyType
    {
        NonBeleiver
    }
    public EnemyType eType;

    public float health;
    [HideInInspector]public Transform bloodPos;

    public bool dead;
    public bool isTarget;

    private void Awake()
    {
        dead = false;
    }

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        lTime = GameController.gc.gameObject.GetComponent<LevelTimer>();
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
        lTime.IncreaseTime(1);
    }

    public void ResetEnemy()
    {
        dead = false;
    }
}
