using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEntertedPositionArea : MonoBehaviour
{
    private bool blockCorrectlyPlaced = false;

    public event Action correctBlockEvent;
    public event Action incorrectBlockEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (!blockCorrectlyPlaced && other.gameObject.tag == gameObject.tag)
        {
            blockCorrectlyPlaced = true;
            correctBlockEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (blockCorrectlyPlaced && other.gameObject.tag == gameObject.tag)
        {
            blockCorrectlyPlaced = false;
            incorrectBlockEvent.Invoke();
        }
    }
}
