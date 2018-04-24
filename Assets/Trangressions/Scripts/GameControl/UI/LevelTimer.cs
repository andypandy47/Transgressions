using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour {

    WinConditions wc;

    Image timerBar;
    public float levelTime;
    public bool debugging;
    [HideInInspector] public float currentTime;

    private void Start()
    {
        RestartTimer();
    }

    public void RestartTimer()
    {
        timerBar = GameObject.FindGameObjectWithTag("TimerBar").GetComponent<Image>();
        wc = GetComponent<WinConditions>();
        currentTime = levelTime;
        UpdateTimerBar();
        print("Timer reset");
    }

    private void Update()
    {
        if (!debugging)
            currentTime -= Time.deltaTime;

        UpdateTimerBar();

        if (currentTime <= 0)
        {
            wc.inTime = false;
        }
        else
            wc.inTime = true;
    }

    void UpdateTimerBar()
    {
        float ratio = Mathf.Clamp(currentTime / levelTime, 0, levelTime);

        if (timerBar != null)
            timerBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        else
        {
            timerBar = GameObject.FindGameObjectWithTag("TimerBar").GetComponent<Image>();
            print("Couldnt find timer bar");
        }
            
    }

    public void IncreaseTime(float amount)
    {
        currentTime += amount;
    }
}
