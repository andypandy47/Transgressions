using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticle : MonoBehaviour {

    ParticleSystem ps;
    public Splatter splatter;

    int splatters;

    int numCollisionEvents;

    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        splatters = 0;
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
                if (!splat.activeInHierarchy)
                    splat.SetActive(true);
            }
            i++;
        }
    }
}
