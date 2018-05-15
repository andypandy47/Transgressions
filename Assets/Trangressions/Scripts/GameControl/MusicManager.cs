using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public static MusicManager instance;

    [FMODUnity.EventRef]
    public string musicEventAudio;
    FMOD.Studio.EventInstance musicEventInstance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicEventInstance = FMODUnity.RuntimeManager.CreateInstance(musicEventAudio);
        musicEventInstance.start();
    }

    public void GamePlayStart()
    {
        musicEventInstance.setParameterValue("PlayingGame", 1);
    }

    public void Progression01()
    {
        musicEventInstance.setParameterValue("Progression01", 1);
    }

    public void Progression02()
    {
        musicEventInstance.setParameterValue("Progression02", 1);
    }

}
