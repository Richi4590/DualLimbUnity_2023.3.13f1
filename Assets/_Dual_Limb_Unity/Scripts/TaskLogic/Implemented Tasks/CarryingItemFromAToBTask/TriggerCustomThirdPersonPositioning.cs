using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCustomThirdPersonPositioning : MonoBehaviour
{
    [SerializeField] private CaryingItemTaskLogic parentTask;
    [SerializeField] private PositionPersonForTask ppft;

    [SerializeField] private GameObject colliderRoot;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "spine")
        {
            colliderRoot.SetActive(true);
            parentTask.TriggerFirstPersonTo(other, ppft);
        }
    }
}
