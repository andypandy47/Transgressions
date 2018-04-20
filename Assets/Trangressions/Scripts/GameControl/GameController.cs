using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController gc;

    Player player;
    public LevelTimer lTime;
    public WinConditions wc;
    BloodEffectsController bEffects;
    GameObject playerObj;
    GameObject UICanvas;
    Transform spawnPoint;

    PauseMenu pMenu;

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
        StartController();
    }

    public void StartController()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        UICanvas = GameObject.FindGameObjectWithTag("UICanvas");
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;

        player = playerObj.GetComponent<Player>();
        pMenu = UICanvas.GetComponent<PauseMenu>();
        lTime = GetComponent<LevelTimer>();
        wc = GetComponent<WinConditions>();
        bEffects = GetComponent<BloodEffectsController>();
            

        print("Reset controller");
    }

    private void Update()
    {

    }

    public bool PlayerLost()
    {
        if (lTime.currentTime <= 0 || !player.alive)
        {
            pMenu.Pause(false);
            UICanvas.transform.GetChild(1).gameObject.SetActive(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlayerWin()
    {
        //WinConditions.allWinConditionsMet = false;
        pMenu.Pause(false);
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

        pMenu.Resume();
        yield return new WaitForSeconds(0.1f);
        player.reset = false;
        
        yield return false;
    }
}
