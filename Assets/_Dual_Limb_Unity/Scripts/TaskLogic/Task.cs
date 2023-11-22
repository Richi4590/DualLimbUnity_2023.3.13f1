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
    [SerializeField] private ConfigurableJointManager jointManager;
    [SerializeField] private GameObject objectTeleporter;

    private UITaskValueReferences UITRef;
    private bool isCompleted = false;

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

    protected void MarkTaskAsCompleted() 
    {
        Debug.Log("Complete!!");
        isCompleted = true;
        UITRef.CompletionStatusObj.SetActive(true);

        if (objectTeleporter != null)
            objectTeleporter.SetActive(false);

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
            Debug.Log("Colliding!!");

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
