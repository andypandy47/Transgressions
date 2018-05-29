﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    Player player;
    PlayerWeaponSystem wSystem;
    PauseMenu pMenu;
    LevelManager lManager;
    InGameUIMenu gameUI;

    void Start()
    {
        player = GetComponent<Player>();
        wSystem = GetComponent<PlayerWeaponSystem>();
        pMenu = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<PauseMenu>();
        lManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        gameUI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<InGameUIMenu>();
    }

    void Update()
    {
        if (!PauseMenu.isPaused && player.alive)
        {
            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            player.SetDirectionalInput(directionalInput);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.OnJumpInputDown();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                player.OnJumpInputUp();
            }
            if (Input.GetMouseButtonDown(1))
            {
                //Right mouse input
                wSystem.RightWeaponInput();
            }
            if (Input.GetMouseButtonDown(0))
            {
                //Left mouse input
                wSystem.LeftWeaponInput();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(lManager.RestartLevel());
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameUI.loseMenu.activeInHierarchy && !gameUI.winMenu.activeInHierarchy)
            {
               if (!PauseMenu.isPaused)
                    pMenu.Pause(true);
                else
                    pMenu.Resume();
            }
            
        }
    }
}