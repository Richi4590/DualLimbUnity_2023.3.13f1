using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingTask : Task
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
            MarkTaskAsCompleted();
    }
}
