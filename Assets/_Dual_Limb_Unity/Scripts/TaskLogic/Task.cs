using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public abstract class Task : MonoBehaviour, ITask
{
    [SerializeField] private string taskName;
    [SerializeField] private string taskDescription;
    [SerializeField] private TaskType taskType;
    [SerializeField] private PositionPersonForTask positioningScript;
    [SerializeField] protected ConfigurableJointManager jointManager;
    [SerializeField] private GameObject taskToEnableAfterCompletion;
    [SerializeField][Space(10)] private GameObject objectTeleporter;
    private bool currentlyDoingTask = false;

    private UITaskValueReferences UITRef;
    private bool isCompleted = false;
    public event Action taskCompleted;

    public string TaskName { get => taskName; }
    public string TaskDescription { get => taskDescription; }
    public TaskType TaskType {get => taskType; }
    public bool IsCompleted { get => isCompleted; }

    private void Start()
    {
        jointManager = GameObject.FindObjectOfType<ConfigurableJointManager>();
    }

    public void SetUIValueReferencer(UITaskValueReferences UIRef)
    {
        UITRef = UIRef;
    }

    public void MarkTaskAsCompleted() 
    {
        currentlyDoingTask = false;
        Debug.Log("Complete!!");
        isCompleted = true;

        if (UITRef != null)
            UITRef.CompletionStatusObj.SetActive(true);

        if (objectTeleporter != null)
            objectTeleporter.SetActive(false);

        if (taskToEnableAfterCompletion != null)
            taskToEnableAfterCompletion.SetActive(true);

        
        GoToThirdPersonMode();

        taskCompleted?.Invoke();
    }

    protected void GoToThirdPersonMode()
    {
        GetComponent<Collider>().enabled = false;
        jointManager.FreezeBody(false);
        InputManager.TogglePersonControlScheme();
        positioningScript.PositioningHelperCamera.enabled = false;
        CameraManager.Instance.SetActiveThirdPersonCamera(true);
        InputManager.ToggleCameraChange(CameraManager.Instance.ThirdPersonCamera);
    }


    private void OnTriggerEnter(Collider other)
    {
        //nputManager.ToggleActionMap()

        if (other.gameObject.name == "spine")
        {
            currentlyDoingTask = true;
            jointManager.FreezeBody(true);
            jointManager.ResetToDefaultBodyPositionAndRotation();
            InputManager.ToggleHandControlScheme();
            InputManager.ToggleCameraChange(positioningScript.PositioningHelperCamera);
            other.gameObject.transform.position = positioningScript.transform.position;
            CameraManager.Instance.SetActiveThirdPersonCamera(false);
            positioningScript.PositioningHelperCamera.enabled = true;
            positioningScript.SetHandsPosition();

            //other.gameObject.transform.parent.rotation = positioningScript.transform.rotation;
        }
    }
}
