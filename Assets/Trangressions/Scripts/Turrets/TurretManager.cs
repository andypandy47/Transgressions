using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour {

    GameObject[] turretsObj;
    public Turret[] turrets;

    private void Start()
    {
        turretsObj = GameObject.FindGameObjectsWithTag("Turret");
        turrets = new Turret[turretsObj.Length];
        for (int i = 0; i < turretsObj.Length; i++)
        {
            turrets[i] = turretsObj[i].GetComponent<Turret>();
        }
    }

    public void ResetTurrets()
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            turrets[i].ResetTurret();
        }
    }

}
