using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    FMOD.Studio.Bus SFXBus;
    FMOD.Studio.Bus LaserTurretsBus;

    void Awake()
    {
        if (instance != this)
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
        SFXBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        LaserTurretsBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX/Turrets/LaserTurrets");
    }

    public void StopTurrets()
    {
        LaserTurretsBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void ResetSFX()
    {
        SFXBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
