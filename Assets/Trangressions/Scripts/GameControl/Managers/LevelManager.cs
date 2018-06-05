using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    Player player;
    PlayerDeath pDeath;
    LevelTimer lTime;
    WinConditions wc;
    BloodEffectsController bEffects;
    InGameUIMenu UIMenus;
    PlatformManager pManager;
    TurretManager tManager;
    ScoreCalculator sCalc;
    PauseMenu pMenu;

    GameObject playerObj;
    GameObject UICanvas;
    
    Transform spawnPoint;
    public float timerInterval;

    private void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        UICanvas = GameObject.FindGameObjectWithTag("UICanvas");
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;

        player = playerObj.GetComponent<Player>();
        pDeath = playerObj.GetComponent<PlayerDeath>();
        pMenu = UICanvas.GetComponent<PauseMenu>();
        UIMenus = UICanvas.GetComponent<InGameUIMenu>();
        lTime = GetComponent<LevelTimer>();
        wc = GetComponent<WinConditions>();
        bEffects = GetComponent<BloodEffectsController>();
        pManager = GetComponent<PlatformManager>();
        tManager = GetComponent<TurretManager>();
        sCalc = GetComponent<ScoreCalculator>();
        
    }

    private void Start()
    {
        print("LManager Start");
        if (GameController.gc.firstRun)
        {
            print("First run");
            StartCoroutine(FirstRunCount());
        }
        else
        {
            UIMenus.DeactivateCountDown();
        }
        CheckScene();
    }

    private void Update()
    {
        if (!wc.playerLost)
            CheckPlayerHasntRunOutOfTime();
    }

    public void CheckPlayerHasntRunOutOfTime()
    {
        if (lTime.currentTime <= 0)
        {
            PlayerLose();
        }
    }

    public void PlayerLose()
    {
        wc.playerLost = true;
        pMenu.Pause(false);
        UIMenus.LoseMenu();
        AudioManager.instance.StopTurrets();
    }

    public void PlayerWin()
    {
        pMenu.Pause(false);
        UIMenus.WinMenu(sCalc.levelCompleteScore, sCalc.CalculateTimeScore(), sCalc.killScore, sCalc.CalculateRank());
        AudioManager.instance.StopTurrets();
    }

    public IEnumerator RestartLevel()
    {
        print("Restart level");

        lTime.RestartTimer();
        wc.ResetConditions();
        bEffects.ResetSprites();
        player.ResetToSpawnPosition(spawnPoint);
        pManager.ResetAllPlatforms();
        tManager.ResetTurrets();
        sCalc.ResetScore();
        ObjectPooler.sharedInstance.DeactivatePooledObjects();
        AudioManager.instance.ResetSFX();

        pMenu.Resume();
        yield return new WaitForSeconds(0.1f);
        player.reset = false;

        yield return false;
    }

    public IEnumerator ResetForNextLevel()
    {
        print("Next Level");
        wc.ResetConditions();
        bEffects.ResetSprites();
        ObjectPooler.sharedInstance.DeactivatePooledObjects();

        yield return false;
    }

    public IEnumerator FirstRunCount()
    {
        pMenu.Pause(false);
        UIMenus.DisplayCountDown("3");
        yield return new WaitForSecondsRealtime(timerInterval);
        UIMenus.DisplayCountDown("2");
        yield return new WaitForSecondsRealtime(timerInterval);
        UIMenus.DisplayCountDown("1");
        yield return new WaitForSecondsRealtime(timerInterval);
        UIMenus.DisplayCountDown("GO!");
        pMenu.Resume();
        GameController.gc.firstRun = false;

        yield return StartCoroutine(UIMenus.FadeCountDownText());
        UIMenus.DisplayCountDown("");
    }

    void CheckScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            MusicManager.instance.Progression01();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            MusicManager.instance.Progression02();
        }
    }


}
