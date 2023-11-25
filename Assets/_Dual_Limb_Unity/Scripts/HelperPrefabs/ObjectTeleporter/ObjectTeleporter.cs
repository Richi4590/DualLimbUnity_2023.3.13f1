using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTeleporter : MonoBehaviour
{
    [SerializeField] private string objectLayerMask;
    [SerializeField] private string tagMask;
    [SerializeField] private bool teleportToHand;
    [SerializeField] private Transform teleportDestination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(objectLayerMask))
        {
            if (other.gameObject.transform.parent != null && !other.gameObject.TryGetComponent<PickUpCollisionEvents>(out PickUpCollisionEvents pce))
            {
                if (other.gameObject.transform.parent.TryGetComponent<Rigidbody>(out Rigidbody rb) ||
                    other.gameObject.transform.parent.TryGetComponent<Collider>(out Collider coll))
                {
                    if (!teleportToHand)
                    {
                        other.gameObject.transform.parent.position = teleportDestination.position;
                        other.gameObject.transform.parent.rotation = teleportDestination.rotation;
                    }
                    else
                    {
                        if (other.gameObject.tag == tagMask && other.gameObject.transform.parent.TryGetComponent<LastHandGrabbed>(out LastHandGrabbed lhg))
                            lhg.ParentAndGrabObjectAgain();
                    }
                }
            }
            else
            {
                if (!teleportToHand)
                {
                    other.gameObject.transform.position = teleportDestination.position;
                    other.gameObject.transform.rotation = teleportDestination.rotation;
                }
                else
                {
                    if (other.gameObject.tag == tagMask && other.gameObject.transform.TryGetComponent<LastHandGrabbed>(out LastHandGrabbed lhg))
                        lhg.ParentAndGrabObjectAgain();
                }

            }

        }
    }

}
 