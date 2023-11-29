using UnityEngine;

public class ExplosionForce : MonoBehaviour
{
    [Header("Explosion Parameters")]
    public float explosionForce = 10f; // Magnitude of the explosion force
    public float explosionRadius = 5f; // Radius of the explosion effect
    public ForceMode forceMode = ForceMode.Impulse; // Type of force applied (Impulse, Force, Acceleration)

    [Header("Optional Settings")]
    public bool affectOnlyRigidbodies = true; // Whether to affect only objects with Rigidbody components
    public LayerMask affectedLayers = Physics.DefaultRaycastLayers; // Layers affected by the explosion
    public bool resetVelocity = false;


    // Function to trigger the explosion force
    public void TriggerExplosion()
    {
        // Find all colliders in the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, affectedLayers);

        if (resetVelocity)
        {
            foreach (Collider collider in colliders)
            {
                // Check if the collider has a Rigidbody component
                bool hasRigidbody = collider.TryGetComponent<Rigidbody>(out Rigidbody rb);

                // If affectOnlyRigidbodies is true, skip non-Rigidbody objects
                if (affectOnlyRigidbodies && !hasRigidbody)
                    continue;

                rb.velocity = Vector3.zero;

                // Calculate the direction from the explosion center to the object
                Vector3 direction = collider.transform.position - transform.position;

                // Calculate the explosion force and apply it to the object
                rb.AddForce(direction.normalized * explosionForce, forceMode);
            }
        }
        else
        {
            foreach (Collider collider in colliders)
            {
                bool hasRigidbody = collider.TryGetComponent<Rigidbody>(out Rigidbody rb);

                // If affectOnlyRigidbodies is true, skip non-Rigidbody objects
                if (affectOnlyRigidbodies && !hasRigidbody)
                    continue;

                // Calculate the direction from the explosion center to the object
                Vector3 direction = collider.transform.position - transform.position;

                // Calculate the explosion force and apply it to the object
                rb.AddForce(direction.normalized * explosionForce, forceMode);
            }
        }


    }

    // You can also call TriggerExplosion from other scripts or events as needed
}