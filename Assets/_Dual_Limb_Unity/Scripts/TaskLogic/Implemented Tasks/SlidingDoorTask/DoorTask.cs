using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTask : Task
{
    [SerializeField] private bool debugDoorPositionValues = false;
    [SerializeField] private GameObject doorParentObj;
    [SerializeField] private GameObject doorSheet;
    [SerializeField] private float doorHalfWidth = 0.1f;
    private float doorFullWidth;
    private Quaternion parentRotation;
    private Vector3 initialPosition;
    private Vector3 destinationDoorPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = doorSheet.transform.position;
        parentRotation = doorParentObj.transform.rotation;

        doorFullWidth = doorHalfWidth * 2;

        Vector3 slideDirection = Quaternion.Euler(0, parentRotation.eulerAngles.y, 0) * Vector3.forward;
        destinationDoorPosition = initialPosition + slideDirection * doorFullWidth;
    }


    void Update()
    {
        if (debugDoorPositionValues)
            Debug.Log("Door current position: " + doorSheet.transform.position + "Door current local position: " + doorSheet.transform.localPosition + "Door destination position >=" + destinationDoorPosition);

        if (doorSheet.transform.position.x >= destinationDoorPosition.x && doorSheet.transform.position.z >= destinationDoorPosition.z)
        {
            if (!IsCompleted)
                MarkTaskAsCompleted();
        }
    }

    private void OnDrawGizmos()
    {
        DrawArrowHelperClass.ForGizmo(doorSheet.transform.position, Vector3.forward * doorHalfWidth);
    }
}
