using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupLogic : MonoBehaviour
{
    [SerializeField] private GameObject coffeePlane;
    [SerializeField] private float coffeePlaneLocalYDestination;
    [SerializeField] private float fillAmountPerFrame = 0.01f;
    private bool cupFilled = false;

    private void OnTriggerStay(Collider other)
    {
        if (!cupFilled && other.gameObject.TryGetComponent<Stream>(out Stream stream))
            StartCoroutine(fillCoffee());
    }

    private IEnumerator fillCoffee()
    {

        float coffeeFillAmount = coffeePlane.transform.localPosition.y + fillAmountPerFrame;

        coffeePlane.transform.localPosition = new Vector3(coffeePlane.transform.localPosition.x, coffeeFillAmount, coffeePlane.transform.localPosition.z);
        
        if (coffeePlane.transform.localPosition.y >= coffeePlaneLocalYDestination)
            cupFilled = true;

        yield return null;
    }
}
