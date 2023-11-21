using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionPersonForTask : MonoBehaviour
{
    [SerializeField] private Camera helperCamera;
    [SerializeField] private Transform TargetHandLeft;
    [SerializeField] private Transform TargetHandRight;

    [SerializeField] private Transform StartingPositionLeft;
    [SerializeField] private Transform StartingPositionRight;

    [SerializeField] private bool setPositionConstantly;

    public Camera PositioningHelperCamera {get => helperCamera; }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetHandsPositionCoroutine());
    }

    public void SetHandsPosition()
    {
        TargetHandLeft.position = StartingPositionLeft.position;
        TargetHandLeft.rotation = StartingPositionLeft.rotation;

        TargetHandRight.position = StartingPositionRight.position;
        TargetHandRight.rotation = StartingPositionRight.rotation;
    }

    private IEnumerator SetHandsPositionCoroutine()
    {
        yield return new WaitForFixedUpdate();

        TargetHandLeft.position = StartingPositionLeft.position;
        TargetHandLeft.rotation = StartingPositionLeft.rotation;

        TargetHandRight.position = StartingPositionRight.position;
        TargetHandRight.rotation = StartingPositionRight.rotation;
    }

    private void Update()
    {
        if (setPositionConstantly)
            StartCoroutine(SetHandsPositionCoroutine());
    }
}
