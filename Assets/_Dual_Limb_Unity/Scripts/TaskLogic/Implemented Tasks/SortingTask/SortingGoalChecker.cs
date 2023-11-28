using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingGoalChecker : MonoBehaviour
{
    [SerializeField] private string tagString;

    [SerializeField] private int totalCorrectObjects;
    private int currentfCorrectObjectsInArea;

    public int CurrentfCorrectObjectsInArea { get { return currentfCorrectObjectsInArea; } }
    public int TotalCorrectObjects { get {  return totalCorrectObjects; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == tagString)
            currentfCorrectObjectsInArea++;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == tagString)
            currentfCorrectObjectsInArea--;
    }
}
