using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHandPositionAfterInit : MonoBehaviour
{

    [SerializeField] private Transform TargetHandLeft;
    [SerializeField] private Transform TargetHandRight;

    [SerializeField] private Transform StartingPositionLeft;
    [SerializeField] private Transform StartingPositionRight;

    [SerializeField] private bool setPositionConstantly;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetHandsCoroutine());
    }


    private IEnumerator SetHandsCoroutine()
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
            StartCoroutine(SetHandsCoroutine());
    }
}
