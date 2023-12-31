using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePushZone : MonoBehaviour
{
    [SerializeField] private float forceStrength = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        DrawArrowHelperClass.ForGizmo(gameObject.transform.position, transform.forward * 0.1f, 0.05f, 20); 
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player") && other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.AddForce(transform.forward * forceStrength, ForceMode.Impulse);
    }
}
