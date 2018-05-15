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
    public GameState gState;

    string currentScene;
    public bool firstRun;

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
        gState = GameState.MainMenu;
        firstRun = true;
    }


}
