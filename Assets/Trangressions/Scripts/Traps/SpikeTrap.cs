using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour {

    Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerCollider")
        {
            StartCoroutine(PlayerDeath.instance.PlayerDead());
        }

        if (collision)
        {
           // print("Trap collided with something " + collision.gameObject.name);
        }
    }
}
