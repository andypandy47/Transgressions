using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticuleFollow : MonoBehaviour {

    LaserTurret turret;

    Transform reticule;

    Vector3 refVector;
    Vector3 currentPosition;
    Vector3 startPos;

    private void Start()
    {
        turret = GetComponent<LaserTurret>();

        reticule = transform.GetChild(1).transform;
        startPos = transform.position;
    }

    public void MoveReticule(Transform target, float reticuleSmoothTime, bool locked)
    {
        if (!locked)
        {
            reticule.position = Vector3.SmoothDamp(reticule.position, target.position, ref refVector, reticuleSmoothTime);
            currentPosition = reticule.position;
        }
        else
        {
            reticule.position = currentPosition;
        }
        
    }

    public void ResetReticule()
    {
        reticule.position = startPos;
    }
}
