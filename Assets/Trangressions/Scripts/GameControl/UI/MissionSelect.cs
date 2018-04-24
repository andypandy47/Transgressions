using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionSelect : MonoBehaviour {


    #region IntroLevels

    public void Intro_Level01()
    {
        SceneManager.LoadScene("Intro_01", LoadSceneMode.Single);
    }

    public void Intro_Level02()
    {
        SceneManager.LoadScene("Intro_02", LoadSceneMode.Single);
    }
    #endregion
}
