using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTask : Task
{
    [SerializeField] private List<BlockEntertedPositionArea> positionList;
    private int numberOfBlocksToBeCorrectlyPlaced;
    private int blocksCorrectlyPlaced;

    void Start()
    {
        if (positionList.Count <= 0)
        {
            Debug.LogError("No position areas for building task assigned!");
        }

        numberOfBlocksToBeCorrectlyPlaced = positionList.Count;

        foreach (var positionPoint in positionList) 
        {
            positionPoint.correctBlockEvent += CorrectBlockEvent;
            positionPoint.incorrectBlockEvent += IncoorrectBlockEvent;
        }

    }

    public void CorrectBlockEvent()
    {
        blocksCorrectlyPlaced++;

        Debug.Log("Blocks correctly placed: " + blocksCorrectlyPlaced);

        if (blocksCorrectlyPlaced == numberOfBlocksToBeCorrectlyPlaced)
            MarkTaskAsCompleted();
    }

    public void IncoorrectBlockEvent()
    {
        blocksCorrectlyPlaced--;
        Debug.Log("Blocks correctly placed: " + blocksCorrectlyPlaced);
    }
}
