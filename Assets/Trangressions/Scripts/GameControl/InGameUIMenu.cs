using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIMenu : MonoBehaviour {

    PauseMenu pMenu;
    GameObject exitPoint;

    private void Awake()
    {
        pMenu = GetComponent<PauseMenu>();
        exitPoint = GameObject.FindGameObjectWithTag("ExitPoint");
    }

    public void RestartLevel()
    {
        /* //Making sure pooled object arent going to be duplicated when reloading level
         for (int i = 0; i < ObjectPooler.sharedInstance.pooledObjects.Count; i++)
         {
             ObjectPooler.sharedInstance.pooledObjects[i].transform.parent = null;

             if (ObjectPooler.sharedInstance.pooledObjects[i].activeInHierarchy)
                 ObjectPooler.sharedInstance.pooledObjects[i].SetActive(false);

             DontDestroyOnLoad(ObjectPooler.sharedInstance.pooledObjects[i]);

         }

         ObjectPooler.sharedInstance.RestartPool();

         SceneManager.LoadScene(SceneManager.GetActiveScene().name);*/

        GameController.gc.StartCoroutine(GameController.gc.RestartLevel());

        // GameController.gc.lTime.RestartTimer();
        // GameController.gc.wc.ResetConditions();
        // GameController.gc.wc.RestartWinConditions();
        // GameController.gc.ResetController();
    }
}
