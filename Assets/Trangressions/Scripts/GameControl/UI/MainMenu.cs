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
    GameObject instructionScreen;
    GameObject missionSelectScreen;

    private void Start()
    {
        instructionScreen = transform.GetChild(1).gameObject;
        missionSelectScreen = transform.GetChild(2).gameObject;
        screen = CurrentScreen.MainMenu;
    }

    public void MissionSelect()
    {
        missionSelectScreen.SetActive(true);
        screen = CurrentScreen.MissionSelect;
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
        else if (screen == CurrentScreen.MissionSelect)
        {
            missionSelectScreen.SetActive(false);
            screen = CurrentScreen.MainMenu;
        }

    }
	
}
