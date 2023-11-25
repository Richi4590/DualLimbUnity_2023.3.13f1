using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTransportArea : MonoBehaviour
{
    [SerializeField] private string tagString;

    public event Action correctItemEntered;
    public event Action correctItemExited;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == tagString)
            correctItemEntered.Invoke();

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == tagString)
            correctItemExited.Invoke(); 
    }
}
