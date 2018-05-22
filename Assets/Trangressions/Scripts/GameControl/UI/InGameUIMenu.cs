using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class InGameUIMenu : MonoBehaviour {

    PauseMenu pMenu;
    LevelManager lManager;
    GameObject exitPoint;

    [HideInInspector]
    public GameObject loseMenu, winMenu, countDownUI;

    TextMeshProUGUI scoreText;
    TextMeshProUGUI timerText;
    TextMeshProUGUI killsText;
    TextMeshProUGUI rankText;

    TextMeshProUGUI countDownText;

    private void Awake()
    {
        pMenu = GetComponent<PauseMenu>();
        exitPoint = GameObject.FindGameObjectWithTag("ExitPoint");
        lManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

        loseMenu = transform.GetChild(1).gameObject;
        winMenu = transform.GetChild(2).gameObject;
        countDownUI = transform.GetChild(4).gameObject;

        scoreText = winMenu.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        timerText = winMenu.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        killsText = winMenu.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        rankText = winMenu.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>();
        countDownText = countDownUI.GetComponent<TextMeshProUGUI>();
        print("UI Start");

    }

    public void RestartLevel()
    {
        StartCoroutine(lManager.RestartLevel());

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

    public void NextLevel()
    {
        StartCoroutine(lManager.ResetForNextLevel());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void DisplayCountDown(string text)
    {
        if (!countDownUI.activeInHierarchy)
            countDownUI.SetActive(true);

        countDownText.text = text;
    }

    public IEnumerator FadeCountDownText()
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            Color c = countDownText.color;
            c.a = f;
            countDownText.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void DeactivateCountDown()
    {
        countDownUI.SetActive(false);
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
