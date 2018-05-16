using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSelection : MonoBehaviour {

    GameObject sectionSelectionScreen;
    GameObject section1SelectScreen;
    GameObject section2SelectScreen;

    private void Start()
    {
        sectionSelectionScreen = transform.GetChild(2).gameObject;
        section1SelectScreen = sectionSelectionScreen.transform.GetChild(3).gameObject;
    }
}
