using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController gc;

    public enum GameState
    {
        MainMenu,
        Options,
        PauseMenu,
        WinMenu,
        LoseMenu,
        TutorialScreen,
        TutorialSection,
        Section1
    }

    Player player;
    public LevelTimer lTime;
    public WinConditions wc;
    BloodEffectsController bEffects;
    InGameUIMenu UIMenus;
    PlatformManager pManager;
    ScoreCalculator sCalc;
    GameObject playerObj;
    GameObject UICanvas;
    GameObject lManager;
    Transform spawnPoint;

    public PauseMenu pMenu;
    string currentScene;

    private void Awake()
    {
        if (!gc)
        {
            gc = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {

    }

  /*  public void StartController()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        UICanvas = GameObject.FindGameObjectWithTag("UICanvas");
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;

        player = playerObj.GetComponent<Player>();
        pMenu = UICanvas.GetComponent<PauseMenu>();
        UIMenus = UICanvas.GetComponent<InGameUIMenu>();
        lTime = lManager.GetComponent<LevelTimer>();
        wc = lManager.GetComponent<WinConditions>();
        bEffects = lManager.GetComponent<BloodEffectsController>();
        pManager = lManager.GetComponent<PlatformManager>();
        sCalc = lManager.GetComponent<ScoreCalculator>();

        print("Reset controller");
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

    public void RestartCurrenLevel()
    {
        lTime.RestartTimer();
        wc.ResetConditions();
        bEffects.ResetSprites();
        player.ResetToSpawnPosition(spawnPoint);

        pMenu.Resume();
        player.reset = false;

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
        ObjectPooler.sharedInstance.ResetSplatters();

        pMenu.Resume();
        yield return new WaitForSeconds(0.1f);
        player.reset = false;
        
        yield return false;
    }

    private void OnLevelWasLoaded(int level)
    {
        
    }*/
}
