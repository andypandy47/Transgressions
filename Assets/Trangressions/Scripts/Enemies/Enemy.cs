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

    public float startingHealth;
    float health;
    [HideInInspector]public Transform bloodPos;

    public bool dead;
    public bool isTarget;
    public GameObject targetPointer;

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

        if (isTarget)
        {
            GameObject pointerObj = Instantiate(targetPointer, transform);
            pointerObj.transform.localPosition = new Vector3(0.06f, 0.359f, 0);
        }
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
        if (isTarget)
            Destroy(transform.GetChild(1).gameObject);

        ScoreCalculator.sCalc.AddEnemyToScore(eType);
        lTime.IncreaseTime(1);
        BloodEffectsController.bFXController.SpawnBloodEffect(this, player);

        dead = true;
        collider.enabled = false;
    }

    public void ResetEnemy()
    {
        health = startingHealth;

        bloodPos.localPosition = new Vector3(0.75f, 0.34f, 0); 
        if (collider.enabled == false)
        {
            collider.enabled = true;
        }

        if (isTarget && dead)
        {
            print("Reset target enemy");
            GameObject pointerObj = Instantiate(targetPointer, transform);
            pointerObj.transform.localPosition = new Vector3(0.06f, 0.359f, 0);
        }
        dead = false;
    }
}
