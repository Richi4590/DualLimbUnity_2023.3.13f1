using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableKinematicAndDisableAfterSomeTime : MonoBehaviour
{
    [SerializeField] private ConfigurableJointManager jointManager;
    [SerializeField] private float secondsToWait = 1.0f;

    // Start is called before the first frame update
    async void Start()
    {
        jointManager.FreezeBody(true);
        await Utilities.WaitForSecondsAsync(secondsToWait);
        jointManager.FreezeBody(false);
    }
}
