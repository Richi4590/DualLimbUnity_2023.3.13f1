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
        //RotatePlayerJointsTorwardsCameraView();
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
        Quaternion q = thirdPersonCamera.transform.rotation;
        q.x = 0; q.z = 0;
        jointManager.RotateJointsTowardsTargetQuaternion(q);
    }
}
