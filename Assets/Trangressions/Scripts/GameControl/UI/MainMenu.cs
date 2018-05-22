using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public enum CurrentScreen
    {
        MainMenu,
        MissionSelect,
        Help,
        Options
    }
    public CurrentScreen screen;
    GameObject mainMenu;
   // GameObject instructionScreen;
    GameObject missionSelectScreen;

    private void Start()
    {
        mainMenu = transform.GetChild(0).gameObject;
        //instructionScreen = transform.GetChild(1).gameObject;
        missionSelectScreen = transform.GetChild(1).gameObject;
        screen = CurrentScreen.MainMenu;
    }

    public void MissionSelect()
    {
        mainMenu.SetActive(false);
        missionSelectScreen.SetActive(true);
        screen = CurrentScreen.MissionSelect;
    }

    public void Help()
    {
        //instructionScreen.SetActive(true);
        screen = CurrentScreen.Help;
    }

    public void Back()
    {
        if (screen == CurrentScreen.Help)
        {
           // instructionScreen.SetActive(false);
            screen = CurrentScreen.MainMenu;
        }
        else if (screen == CurrentScreen.MissionSelect)
        {
            missionSelectScreen.SetActive(false);
            mainMenu.SetActive(true);
            screen = CurrentScreen.MainMenu;
        }

    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }
	
}
