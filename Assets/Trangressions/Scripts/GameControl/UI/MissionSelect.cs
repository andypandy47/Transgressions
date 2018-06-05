using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionSelect : MonoBehaviour {

    UISounds UISounds;

    private void Start()
    {
        UISounds = GetComponent<UISounds>();
    }

    public void SelectMission(string level)
    {
        //TODO: Check to see if level is actually playable
        //if not then play error sound instead
        UISounds.LevelSelectSound();
        SceneManager.LoadScene(level, LoadSceneMode.Single);
        MusicManager.instance.GamePlayStart();
    }

}
