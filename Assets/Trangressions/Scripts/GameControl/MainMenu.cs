using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public enum CurrentScreen
    {
        MainMenu,
        Help,
        Options
    }
    public CurrentScreen screen;
    GameObject instructionScreen;

    private void Start()
    {
        instructionScreen = transform.GetChild(1).gameObject;
        screen = CurrentScreen.MainMenu;
    }

    public void TutorialStart()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    public void Help()
    {
        instructionScreen.SetActive(true);
        screen = CurrentScreen.Help;
    }

    public void Back()
    {
        if (screen == CurrentScreen.Help)
        {
            instructionScreen.SetActive(false);
            screen = CurrentScreen.MainMenu;
        }
            
    }
	
}
