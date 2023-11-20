using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<Task> tasks;

    // Start is called before the first frame update
    void Start()
    {
        if (tasks.Count <= 0) 
        {
            Debug.LogError("No tasks for Level assigned!");
        }

        // generate UI out of all tasks in array
    }

    private void DisplayAllTasksDone()
    {
        // UI all tasks done maybe UI
    }
}
