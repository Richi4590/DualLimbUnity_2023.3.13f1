using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTeleporter : MonoBehaviour
{
    [SerializeField] private string objectLayerMask;
    [SerializeField] private Transform teleportDestination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(objectLayerMask))
        {
            if (other.gameObject.transform.parent != null)
            {
                if (other.gameObject.transform.parent.TryGetComponent<Rigidbody>(out Rigidbody rb) ||
                    other.gameObject.transform.parent.TryGetComponent<Collider>(out Collider coll))
                {
                    other.gameObject.transform.parent.position = teleportDestination.position;
                    other.gameObject.transform.parent.rotation = teleportDestination.rotation;
                }
            }
            else
            {
                other.gameObject.transform.position = teleportDestination.position;
                other.gameObject.transform.rotation = teleportDestination.rotation;
            }

        }
    }

}
 