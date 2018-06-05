using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour {

    [FMODUnity.EventRef]
    public string selectSound;
    [FMODUnity.EventRef]
    public string backSound;
    [FMODUnity.EventRef]
    public string errorSound;
    [FMODUnity.EventRef]
    public string levelSelectSound;
    [FMODUnity.EventRef]
    public string highlightSound;

    public void SelectSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(selectSound, transform.position);
    }

    public void BackSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(backSound, transform.position);
    }

    public void ErrorSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(errorSound, transform.position);
    }

    public void LevelSelectSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(levelSelectSound, transform.position);
    }

    public void HighlightSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(highlightSound, transform.position);
    }
}
