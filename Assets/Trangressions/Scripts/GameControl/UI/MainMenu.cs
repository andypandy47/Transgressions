using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public enum CurrentScreen
    {
        MainMenu,
        SectionSelection,
        MissionSelection,
        Help,
        Options
    }
    public CurrentScreen screen;
    GameObject instructionScreen;
    GameObject sectionSelectionScreen;
    public GameObject[] missionSelectScreens;

    private void Start()
    {
        instructionScreen = transform.GetChild(1).gameObject;
        sectionSelectionScreen = transform.GetChild(2).gameObject;

        screen = CurrentScreen.MainMenu;
    }

    public void SectionSelect()
    {
        sectionSelectionScreen.SetActive(true);
        for (int i = 0; i < 2; i++)
        {
            if (!sectionSelectionScreen.transform.GetChild(i).gameObject.activeInHierarchy)
                sectionSelectionScreen.transform.GetChild(i).gameObject.SetActive(true);
            else
                continue;
        }
        screen = CurrentScreen.SectionSelection;
    }

    public void Section1Select()
    {
        for (int i = 0; i < 2; i++)
        {
            sectionSelectionScreen.transform.GetChild(i).gameObject.SetActive(false);
        }
        missionSelectScreens[0].SetActive(true);
        screen = CurrentScreen.MissionSelection;
        print("Section 1 select scree");
    }

    public void Section2Select()
    {
        screen = CurrentScreen.MissionSelection;
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
        else if (screen == CurrentScreen.SectionSelection)
        {
            sectionSelectionScreen.SetActive(false);
            screen = CurrentScreen.MainMenu;
        }
        else if (screen == CurrentScreen.MissionSelection)
        {
            for (int i = 0; i < missionSelectScreens.Length; i++)
            {
                if (missionSelectScreens[i].activeInHierarchy)
                    missionSelectScreens[i].SetActive(false);
                else
                    continue;
            }
            SectionSelect();
        }

    }
	
}
