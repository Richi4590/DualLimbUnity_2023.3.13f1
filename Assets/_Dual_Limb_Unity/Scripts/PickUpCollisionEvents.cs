using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCollisionEvents : MonoBehaviour
{
    public GameObject ObjectInPickUpRange;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pickup"))
        {
            ObjectInPickUpRange = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pickup"))
            ObjectInPickUpRange = null;
    }
}
