using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamShake : MonoBehaviour {

    public static CamShake instance;
    CinemachineVirtualCamera vCam;
    CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        if (instance == null)
            instance = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<CamShake>();

        vCam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public IEnumerator VirutalCameraShake(float shakeIntensity, float shakeTiming)
    {
        print("Camshake called");
        Noise(.1f, shakeIntensity);
        yield return new WaitForSeconds(shakeTiming);
        Noise(0, 0);
    }

    void Noise(float amplitudeGain, float frequencyGain)
    {
        noise.m_AmplitudeGain = amplitudeGain;
        noise.m_FrequencyGain = frequencyGain;
    }


}
