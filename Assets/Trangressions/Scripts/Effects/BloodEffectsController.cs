using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffectsController : MonoBehaviour {

    public static BloodEffectsController bFXController;

    public GameObject[] bloodEffects;

    private void Awake()
    {
        if (bFXController == null)
            bFXController = this;
    }

    public void SpawnBloodEffect(Enemy enemy, Player player)
    {
        bool rightOfEnemy = false;

        //Check if player is right of enemy so that we know which way to spawn the blood effect
        if (player.transform.position.x > enemy.transform.position.x)
            rightOfEnemy = true;
        else
            rightOfEnemy = false;

        GameObject effect = Instantiate(bloodEffects[0], enemy.transform);

        if (rightOfEnemy)
        {
            enemy.bloodPos.localPosition = new Vector3(-enemy.bloodPos.localPosition.x, enemy.bloodPos.localPosition.y, 0);
            Flip(effect);
        }
            

        effect.transform.position = enemy.bloodPos.position;

    }

    void Flip(GameObject sprite)
    {
        sprite.transform.localScale = new Vector3(-1, 1, 1);
    }
}
