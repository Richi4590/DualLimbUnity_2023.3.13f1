using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayerTowardsCameraView : MonoBehaviour
{
    [SerializeField] private GameObject playerRoot;
    [SerializeField] private ConfigurableJointManager jointManager;
    [SerializeField] private Camera thirdPersonCamera;

    private void FixedUpdate()
    {
        RotatePlayerJointsTorwardsCameraView();
    }

    private void RotatePlayerTorwardsCameraView()
    {
        Quaternion q = thirdPersonCamera.transform.rotation;
        q.x = playerRoot.transform.rotation.x;
        q.z = playerRoot.transform.rotation.z;
        playerRoot.transform.rotation = q;
    }

    private void RotatePlayerJointsTorwardsCameraView()
    {
        float cameraYRotation = thirdPersonCamera.transform.eulerAngles.y + 90;
        // Create a new rotation using the current rotation of the configurable joint and the Y rotation of the camera
        Quaternion q = Quaternion.Euler(0, -cameraYRotation, 0);

        jointManager.RotateJointsTowardsTargetQuaternion(q);
    }
}
