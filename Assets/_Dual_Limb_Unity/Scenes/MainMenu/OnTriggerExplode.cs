using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerExplode : MonoBehaviour
{
    [SerializeField] ExplosionForce explosionPoint;
    [SerializeField] ConfigurableJointManager jointManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            jointManager.DoRagdoll(true);
            explosionPoint.TriggerExplosion();
        }
            
    }
}
