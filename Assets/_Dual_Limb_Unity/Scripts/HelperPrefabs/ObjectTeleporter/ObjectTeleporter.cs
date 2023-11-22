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
            other.gameObject.transform.position = teleportDestination.position;
            other.gameObject.transform.rotation = teleportDestination.rotation;
        }
    }

}
 