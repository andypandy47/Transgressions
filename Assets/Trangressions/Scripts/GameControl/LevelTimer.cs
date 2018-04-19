using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour {

    public static LevelTimer lTime;

    public Image timerBar;
    public float levelTime;
    [HideInInspector] public float currentTime;

    private void Awake()
    {
        if (lTime == null)
            lTime = this;
    }

    private void Start()
    {
        currentTime = levelTime;
        UpdateTimerBar();
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        UpdateTimerBar();

        if (currentTime <= 0)
        {
            WinConditions.wc.inTime = false;
        }
        else
            WinConditions.wc.inTime = true;
    }

    void UpdateTimerBar()
    {
        float ratio = Mathf.Clamp(currentTime / levelTime, 0, levelTime);
        timerBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }

    public void IncreaseTime(float amount)
    {
        currentTime += amount;
    }
}
