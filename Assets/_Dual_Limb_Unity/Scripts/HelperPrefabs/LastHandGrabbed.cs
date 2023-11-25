using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastHandGrabbed : MonoBehaviour
{
    public PlayerControlAndPhysicsPickUp lastPlayerController;
    public Vector3 lastValidGrabbedLocalPosition;
    public Quaternion lastValidGrabbedRotation;

    public void ParentAndGrabObjectAgain()
    {
        lastPlayerController.ParentAndGrabImportantFallenObjectAgain(GetComponent<Rigidbody>(), lastValidGrabbedLocalPosition, lastValidGrabbedRotation);
    }

}
