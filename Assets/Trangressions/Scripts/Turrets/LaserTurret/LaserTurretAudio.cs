using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurretAudio : MonoBehaviour {

    [FMODUnity.EventRef]
    public string laserSoundEvent;
    FMOD.Studio.EventInstance laserSound;

    [HideInInspector] public bool isPlaying;

    private void Start()
    {
        ResetTurretAudio();
    }

    public void ResetTurretAudio()
    {
        laserSound = FMODUnity.RuntimeManager.CreateInstance(laserSoundEvent);
        isPlaying = false;
    }

    public void StartLaserSound()
    {
        laserSound.start();
        isPlaying = true;
    }

    public void StopLaser()
    {
        laserSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        isPlaying = false;
    }

    public bool SoundIsPlaying()
    {
        FMOD.Studio.PLAYBACK_STATE playbackState;
        laserSound.getPlaybackState(out playbackState);
        if (playbackState == FMOD.Studio.PLAYBACK_STATE.STARTING || playbackState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            return true;
        else
            return false;
    }

}
