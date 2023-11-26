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

    // Function to trigger the explosion force
    public void TriggerExplosion()
    {
        // Find all colliders in the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, affectedLayers);

        foreach (Collider collider in colliders)
        {
            // Check if the collider has a Rigidbody component
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            // If affectOnlyRigidbodies is true, skip non-Rigidbody objects
            if (affectOnlyRigidbodies && rb == null)
                continue;

            // Calculate the direction from the explosion center to the object
            Vector3 direction = collider.transform.position - transform.position;

            // Calculate the explosion force and apply it to the object
            rb.AddForce(direction.normalized * explosionForce, forceMode);
        }
    }

    // You can also call TriggerExplosion from other scripts or events as needed
}