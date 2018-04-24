using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class InGameUIMenu : MonoBehaviour {

    PauseMenu pMenu;
    GameObject exitPoint;

    GameObject loseMenu;
    GameObject winMenu;

    TextMeshProUGUI scoreText;
    TextMeshProUGUI timerText;
    TextMeshProUGUI killsText;
    TextMeshProUGUI rankText;

    private void Start()
    {
        pMenu = GetComponent<PauseMenu>();
        exitPoint = GameObject.FindGameObjectWithTag("ExitPoint");

        loseMenu = transform.GetChild(1).gameObject;
        winMenu = transform.GetChild(2).gameObject;

        scoreText = winMenu.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        timerText = winMenu.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        killsText = winMenu.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        rankText = winMenu.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>();
    }

    public void RestartLevel()
    {
        GameController.gc.StartCoroutine(GameController.gc.RestartLevel());

        if (loseMenu.activeInHierarchy)
        {
            loseMenu.SetActive(false);
        }

        if (winMenu.activeInHierarchy)
        {
            winMenu.SetActive(false);
            ResetScoreText();
        }
    }

    public void LoseMenu()
    {
        if (!loseMenu.activeInHierarchy)
        {
            loseMenu.SetActive(true);
        }
    }

    public void WinMenu(int levelScore, int timeScore, int killScore, string rank)
    {
        if (!winMenu.activeInHierarchy)
        {
            winMenu.SetActive(true);
            StartCoroutine(WinMenuScores(levelScore, timeScore, killScore, rank));
        }
    }

    IEnumerator WinMenuScores(int levelScore, int timeScore, int killScore, string rank)
    {
        scoreText.text = levelScore.ToString();
        yield return new WaitForSecondsRealtime(0.2f);
        timerText.text = timeScore.ToString();
        yield return new WaitForSecondsRealtime(0.2f);
        killsText.text = killScore.ToString();
        yield return new WaitForSecondsRealtime(0.2f);
        rankText.text = rank;

        yield return null;
    }

    void ResetScoreText()
    {
        scoreText.text = "";
        timerText.text = "";
        killsText.text = "";
        rankText.text = "";
    }
}
