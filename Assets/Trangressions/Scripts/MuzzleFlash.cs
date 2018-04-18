using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {

    PlayerWeaponSystem wSystem;

    float timeToDisable = 0.2f;
    float timer;

    private void Start()
    {
        wSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeaponSystem>();
        
    }

    private void OnEnable()
    {
        timer = timeToDisable;

        if (transform.localScale.x == -1)
            Flip();
    }

    private void Update()
    {
        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
