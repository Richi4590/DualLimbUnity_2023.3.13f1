using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupLogic : MonoBehaviour
{

    [SerializeField] private GameObject coffeePlane;
    [SerializeField] private float coffeePlaneLocalYDestination;
    [SerializeField] private float fillAmountPerFrame = 0.01f;
    [SerializeField] private GameObject particlesObj;
    private Material particleMat;

    private bool cupFilled = false;
    private float startYCoordCoffeePlane;

    public bool CupFilled { get => cupFilled; }
    public event Action<CupLogic> cupDoneEvent;

    private void Start()
    {
        startYCoordCoffeePlane = coffeePlane.transform.localPosition.y;
        particleMat = particlesObj.GetComponent<Renderer>().material;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!cupFilled && other.gameObject.TryGetComponent<Stream>(out Stream stream))
            StartCoroutine(fillCoffee());
    }

    private IEnumerator fillCoffee()
    {
        if (!particlesObj.activeSelf)
            particlesObj.SetActive(true);

        Color oldColor = particleMat.GetColor("_Color");
        particleMat.SetColor("_Color", new Color(oldColor.r, oldColor.g, oldColor.b, InterpolateAlphaColorParticles()));

        float coffeeFillAmount = coffeePlane.transform.localPosition.y + fillAmountPerFrame;

        coffeePlane.transform.localPosition = new Vector3(coffeePlane.transform.localPosition.x, coffeeFillAmount, coffeePlane.transform.localPosition.z);
        
        if (coffeePlane.transform.localPosition.y >= coffeePlaneLocalYDestination)
        {
            cupFilled = true;
            cupDoneEvent.Invoke(this);
        }


        yield return null;
    }

    private float InterpolateAlphaColorParticles()
    {
        return Mathf.InverseLerp(startYCoordCoffeePlane, coffeePlaneLocalYDestination, coffeePlane.transform.localPosition.y);
    }

    private float MapValue(float value, float originalMin, float originalMax, float targetMin, float targetMax)
    {
        // Map the value from the original range to the target range
        return Mathf.Lerp(targetMin, targetMax, Mathf.InverseLerp(originalMin, originalMax, value));
    }


}
