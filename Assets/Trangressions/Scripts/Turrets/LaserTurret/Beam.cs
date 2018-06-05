using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour {

    float offSet;
    Color transparent = new Color(0, 0, 0, 0);
    LaserTurret lTurret;
    LaserTurretAudio lAudio;

    private void Start()
    {
        lTurret = GetComponent<LaserTurret>();
        lAudio = GetComponent<LaserTurretAudio>();
    }

    public IEnumerator ShootLaser(Transform player, LineRenderer lineRend, Vector3 shootDir, Vector3 firePoint, float duration,
        LayerMask unitMask, LayerMask wallMask, bool reset)
    {
        if (lTurret.state == LaserTurret.State.Firing)
        {
            print("Shoot beam");
            RaycastHit2D wallHit = Physics2D.Raycast(firePoint, shootDir, Mathf.Infinity, wallMask);
            float rayDistance = wallHit.distance;

            lineRend.material.SetColor("_Color", Color.white);
            lineRend.SetPosition(0, firePoint);
            lineRend.SetPosition(1, wallHit.point);

            StartCoroutine(CamShake.instance.VirutalCameraShake(10, .5f));

            float timer = 0.0f;
            while (timer < duration && !reset)
            {
                RaycastHit2D unitHit = Physics2D.Raycast(firePoint, shootDir, rayDistance, unitMask);
                if (unitHit)
                {
                    if (unitHit.collider.tag == "PlayerCollider")
                    {
                        CapsuleCollider2D playerCollider = unitHit.collider.GetComponent<CapsuleCollider2D>();
                        playerCollider.enabled = false;

                        StartCoroutine(PlayerDeath.instance.PlayerDead());
                        StartCoroutine(CamShake.instance.VirutalCameraShake(10, .2f));
                        print("laser hit player");
                    }
                    else if (unitHit.collider.tag == "Enemy")
                    {
                        print("Enemy hit by laser");
                    }
                }

                offSet += Time.deltaTime;
                lineRend.material.SetTextureOffset("_MainTex", new Vector2(offSet, 0));

                timer += Time.deltaTime;
                yield return null;
            }
        }
        

        StartCoroutine(FadeLaser(lineRend, firePoint));
        
    }

    IEnumerator FadeLaser(LineRenderer lineRend, Vector3 firePoint)
    {
        for (float f = 1f; f > 0; f -= 0.1f)
        {
            Color c = lineRend.material.color;
            c.a = f;
            lineRend.material.SetColor("_Color", c);
            yield return new WaitForSeconds(0.03f);
        }
        lineRend.SetPosition(1, firePoint);
        lineRend.material.SetColor("_Color", transparent);
    }

    public void ResetBeam(LineRenderer lineRend, Vector3 firePoint)
    {
        print("Reset beam");
        lineRend.SetPosition(1, firePoint);
        lineRend.material.SetColor("_Color", transparent);
    }
}
