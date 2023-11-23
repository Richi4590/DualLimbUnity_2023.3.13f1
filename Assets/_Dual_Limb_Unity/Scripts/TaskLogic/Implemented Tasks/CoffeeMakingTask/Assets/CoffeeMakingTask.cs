using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMakingTask : Task
{
    [SerializeField] private List<CupLogic> coffeeCups;
    private int cupsToFill;
    private int cupsFilled = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (coffeeCups.Count <= 0)
        {
            Debug.LogError("No cups for coffee pouring task assigned!");
        }

        cupsToFill = coffeeCups.Count;

        foreach (var cup in coffeeCups)
            cup.cupDoneEvent += CupFilledEvent;
    }

    public void CupFilledEvent(CupLogic logic)
    {
        cupsFilled++;

        if (cupsFilled == cupsToFill)
            MarkTaskAsCompleted();
    }
}
