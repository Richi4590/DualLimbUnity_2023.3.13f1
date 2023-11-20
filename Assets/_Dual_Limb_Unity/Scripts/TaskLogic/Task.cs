using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public abstract class Task : MonoBehaviour, ITask
{
    [SerializeField] private PositionPersonForTask positioningScript;
    [SerializeField] private ConfigurableJointManager jointManager;

    [SerializeField] private string taskName;
    [SerializeField] private string taskDescription;
    [SerializeField] private TaskType taskType;
    private bool isCompleted = false;

    public string TaskName { get => taskName; }
    public string TaskDescription { get => taskDescription; }
    public TaskType TaskType {get => taskType; }
    public bool IsCompleted { get => isCompleted; }


    private void Start()
    {

    }

    private void MarkTaskAsCompleted() { isCompleted = true; }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "spine")
        {
            jointManager.FreezeBody(true);
            positioningScript.SetHandsPosition();
            other.gameObject.transform.position = positioningScript.transform.position;
            other.gameObject.transform.parent.rotation = positioningScript.transform.rotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isCompleted)
        {
            GetComponent<Collider>().enabled = false;
        }
    }
}
