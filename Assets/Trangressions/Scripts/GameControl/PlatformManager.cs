using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    GameObject[] platformObjects;
    PlatformController[] platforms;

    private void Start()
    {
        platformObjects = GameObject.FindGameObjectsWithTag("Platform");
        platforms = new PlatformController[platformObjects.Length];

        for (int i = 0; i < platformObjects.Length; i++)
        {
            platforms[i] = platformObjects[i].GetComponent<PlatformController>();
        }
    }

    public void ResetAllPlatforms()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].StartCoroutine(platforms[i].ResetPlatform());
        }
    }
}
