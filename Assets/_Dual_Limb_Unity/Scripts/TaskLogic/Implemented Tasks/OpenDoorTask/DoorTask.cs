using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTask : Task
{
    [SerializeField] private GameObject door;
    private Vector3 destinationDoorPosition;

    // Start is called before the first frame update
    void Start()
    {
        destinationDoorPosition = door.transform.forward;
        destinationDoorPosition = destinationDoorPosition * (door.transform.position.x + door.GetComponent<MeshRenderer>().bounds.size.x);
    }


    void Update()
    {
        if (door.transform.position.x >= destinationDoorPosition.x && door.transform.position.z >= destinationDoorPosition.z)
        {
            if (!IsCompleted)
                MarkTaskAsCompleted();
        }
    }
}
