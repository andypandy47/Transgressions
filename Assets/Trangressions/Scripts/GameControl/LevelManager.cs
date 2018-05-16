using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    Player player;
    LevelTimer lTime;
    WinConditions wc;
    BloodEffectsController bEffects;
    InGameUIMenu UIMenus;
    PlatformManager pManager;
    ScoreCalculator sCalc;
    PauseMenu pMenu;

    GameObject playerObj;
    GameObject UICanvas;
    Transform spawnPoint;

    private void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        UICanvas = GameObject.FindGameObjectWithTag("UICanvas");
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;

        player = playerObj.GetComponent<Player>();
        pMenu = UICanvas.GetComponent<PauseMenu>();
        UIMenus = UICanvas.GetComponent<InGameUIMenu>();
        lTime = GetComponent<LevelTimer>();
        wc = GetComponent<WinConditions>();
        bEffects = GetComponent<BloodEffectsController>();
        pManager = GetComponent<PlatformManager>();
        sCalc = GetComponent<ScoreCalculator>();
    }

    private void Update()
    {
        if (!wc.playerLost)
            CheckPlayerLost();
    }

    public void CheckPlayerLost()
    {
        if (lTime.currentTime <= 0 || !player.alive)
        {
            wc.playerLost = true;
            pMenu.Pause(false);
            UIMenus.LoseMenu();
        }
    }

    public void PlayerWin()
    {
        pMenu.Pause(false);
        UIMenus.WinMenu(sCalc.levelCompleteScore, sCalc.CalculateTimeScore(), sCalc.killScore, sCalc.CalculateRank());
    }

    public IEnumerator RestartLevel()
    {
        print("Restart level");

        lTime.RestartTimer();
        wc.ResetConditions();
        bEffects.ResetSprites();
        player.ResetToSpawnPosition(spawnPoint);
        pManager.ResetAllPlatforms();
        sCalc.ResetScore();
        ObjectPooler.sharedInstance.DeactivatePooledObjects();

        pMenu.Resume();
        yield return new WaitForSeconds(0.1f);
        player.reset = false;

        yield return false;
    }

    public IEnumerator NextLevel()
    {
        print("Next Level");
        wc.ResetConditions();
        bEffects.ResetSprites();
        ObjectPooler.sharedInstance.DeactivatePooledObjects();

        yield return false;
    }


}
