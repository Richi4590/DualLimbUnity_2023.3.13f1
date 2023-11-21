using EazyCamera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; } 

    [SerializeField] private Camera thirdPersonCamera;
    [SerializeField] private Camera uIIconsCamera;
    [SerializeField] private EazyController easyCameraController;
    [SerializeField] private EazyCam ezCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public Camera ThirdPersonCamera { get => thirdPersonCamera; }
    public Camera UIIconsCamera { get => uIIconsCamera; }

    public void SetActiveThirdPersonCamera(bool state)
    {
        Instance.ezCamera.enabled = state;
        Instance.easyCameraController.enabled = state;
        Instance.uIIconsCamera.enabled = state;
        Instance.ThirdPersonCamera.enabled = state;
    }
}
