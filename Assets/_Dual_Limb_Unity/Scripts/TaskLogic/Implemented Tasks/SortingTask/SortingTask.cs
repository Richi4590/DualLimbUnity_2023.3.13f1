using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingTask : Task
{
    [SerializeField] private SortingGoalChecker redChecker;
    [SerializeField] private SortingGoalChecker blueChecker;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    void Update()
    {
        if (redChecker.CurrentfCorrectObjectsInArea == redChecker.TotalCorrectObjects &&
            blueChecker.CurrentfCorrectObjectsInArea == blueChecker.TotalCorrectObjects)
        {
            if (!IsCompleted)
                MarkTaskAsCompleted();
        }
    }
}
