using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffectsController : MonoBehaviour {

    public static BloodEffectsController bFXController;

    public GameObject[] bloodEffects;
    public GameObject bloodPSystem;
    public List<GameObject> activeBloodSprites;

    private void Awake()
    {
        if (bFXController == null)
            bFXController = this;
    }

    public void SpawnBloodEffect(Enemy enemy, Player player)
    {
        bool rightOfEnemy = false;
        float enemyDirX = enemy.transform.localScale.x;

        //Check if player is right of enemy so that we know which way to spawn the blood effect
        if (player.transform.position.x > enemy.transform.position.x)
            rightOfEnemy = true;
        else
            rightOfEnemy = false;

        GameObject effect = Instantiate(bloodEffects[0], enemy.transform);
        GameObject pSystem = Instantiate(bloodPSystem, enemy.transform);

        if (rightOfEnemy && enemyDirX == 1)
        {
            enemy.bloodPos.localPosition = new Vector3(-enemy.bloodPos.localPosition.x, enemy.bloodPos.localPosition.y, 0);
            pSystem.transform.eulerAngles = new Vector3(0, -90, 0);
            Flip(effect);
        }
        else if (!rightOfEnemy && enemyDirX == -1)
        {
            enemy.bloodPos.localPosition = new Vector3(-enemy.bloodPos.localPosition.x, enemy.bloodPos.localPosition.y, 0);
            pSystem.transform.eulerAngles = new Vector3(0, 90, 0);
            Flip(effect);
        }

        effect.transform.position = enemy.bloodPos.position;
        pSystem.transform.position = enemy.transform.position;
        activeBloodSprites.Add(effect);
        Destroy(pSystem, 1);
    }

    void Flip(GameObject sprite)
    {
        sprite.transform.localScale = new Vector3(-1, 1, 1);
    }

    public void ResetSprites()
    {
        //TODO: Pool ur blood sprites m8
        for (int i = 0; i < activeBloodSprites.Count; i++)
        {
            Destroy(activeBloodSprites[i]);
        }
        activeBloodSprites.Clear();
    }
}
