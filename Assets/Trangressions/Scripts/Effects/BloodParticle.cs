using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticle : MonoBehaviour {

    ParticleSystem ps;
    public Splatter splatter;
    Player player;

    int splatters;

    int numCollisionEvents;

    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    [FMODUnity.EventRef]
    public string splatAudioEvent;
    FMOD.Studio.EventInstance splatEvent;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        splatters = 0;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int safeLength = ParticlePhysicsExtensions.GetSafeCollisionEventSize(ps);
        if (collisionEvents.Count < safeLength)
            collisionEvents = new List<ParticleCollisionEvent>();

        numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(ps, other, collisionEvents);

        int i = 0;
        while (i < numCollisionEvents)
        {
            Vector3 collisionHitLoc = collisionEvents[i].intersection;
            GameObject splat = ObjectPooler.sharedInstance.GetSplatter("Splatter");

            if (splat != null)
            {
                splat.transform.position = collisionHitLoc;
                splat.transform.rotation = Quaternion.identity;

                float distance = Vector3.Distance(collisionHitLoc, player.transform.position);
                float distancePercent = (distance / 20);
                distancePercent = Mathf.Clamp01(distancePercent);
                splatEvent = FMODUnity.RuntimeManager.CreateInstance(splatAudioEvent);
                splatEvent.setParameterValue("Distance", distancePercent);
                splatEvent.start();
                splatEvent.release();
                
                if (collisionEvents[i].colliderComponent.tag == "Platform")
                {
                    splat.transform.parent = collisionEvents[i].colliderComponent.transform;
                }

                if (!splat.activeInHierarchy)
                    splat.SetActive(true);
            }
            i++;
        }
    }
}
