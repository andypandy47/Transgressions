using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {

    LevelManager lManager;
    public SpriteRenderer[] sRend = new SpriteRenderer[3];
    Player player;

    public static PlayerDeath instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<PlayerDeath>();
        }
    }

    private void Start()
    {
        lManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        player = GetComponent<Player>();
    }

    public IEnumerator PlayerDead()
    {
        for (int i = 0; i < sRend.Length; i++)
        {
            sRend[i].enabled = false;
        }

        player.alive = false;

        GameObject chunkEffect = ObjectPooler.sharedInstance.GetPooledObject("PlayerDeathChunks");
        chunkEffect.transform.position = transform.position;
        chunkEffect.SetActive(true);
        lManager.PlayerLose();
        print("Playerdead");
        yield return new WaitForSeconds(3f);
        chunkEffect.SetActive(false);

    }
}
