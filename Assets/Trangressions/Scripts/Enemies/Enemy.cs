﻿using System.Collections;
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

    public float startingHealth;
    float health;
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

        health = startingHealth;
    }

    public void DamageEnemy(float damage, Player player)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            KillEnemy(player);
        }
    }

    public void KillEnemy(Player player)
    {
        dead = true;
        collider.enabled = false;
        ScoreCalculator.sCalc.AddEnemyToScore(eType);
        lTime.IncreaseTime(1);
        BloodEffectsController.bFXController.SpawnBloodEffect(this, player);
    }

    public void ResetEnemy()
    {
        dead = false;
        health = startingHealth;

        bloodPos.localPosition = new Vector3(0.75f, 0.34f, 0); 
        if (collider.enabled == false)
        {
            collider.enabled = true;
        }
    }
}
