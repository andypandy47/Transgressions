using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public static bool isPaused = false;
    GameObject pauseMenuUI;

    private void Start()
    {
        pauseMenuUI = transform.GetChild(0).gameObject;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
        print("Resume");
    }

    public void Pause(bool withMenu)
    {
        if (withMenu)
            pauseMenuUI.SetActive(true);

        isPaused = true;
        Time.timeScale = 0;
        print("Pause");
    }
}
