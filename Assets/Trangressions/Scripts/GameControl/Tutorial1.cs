using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1 : MonoBehaviour {

    GameObject UICanvas;
   // GameObject instructionMenu;
    PauseMenu pMenu;

    private void Start()
    {
        UICanvas = GameObject.FindGameObjectWithTag("UICanvas");
        //instructionMenu = transform.GetChild(4).gameObject;

        pMenu = GetComponent<PauseMenu>();
        //pMenu.Pause(false);
        //StartCoroutine(TutorialStart());
    }

    public IEnumerator TutorialStart()
    {
        
        yield return null;
    }
}
