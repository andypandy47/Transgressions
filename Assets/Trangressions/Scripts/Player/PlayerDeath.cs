using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {

    LevelManager lManager;
    SpriteRenderer sRend;
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
        sRend = GetComponent<SpriteRenderer>();
        lManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        player = GetComponent<Player>(); 
    }

    public IEnumerator PlayerDead()
    {
        sRend.enabled = false;
        player.alive = false;

        GameObject chunkEffect = ObjectPooler.sharedInstance.GetPooledObject("PlayerDeathChunks");
        chunkEffect.transform.position = transform.position;
        chunkEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        lManager.PlayerLose();

    }
}
