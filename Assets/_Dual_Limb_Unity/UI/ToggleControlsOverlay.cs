using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Controls
{
    person,
    hand
}

public class ToggleControlsOverlay : MonoBehaviour
{
    public GameObject personUI;
    public GameObject handUI;

    public static ToggleControlsOverlay Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void ShowControlsOverlayFor(Controls controls) 
    {
        switch (controls)
        {
            case Controls.person:
                Instance.personUI.SetActive(true);
                Instance.handUI.SetActive(false);
                break;
            case Controls.hand:
                Instance.personUI.SetActive(false);
                Instance.handUI.SetActive(true);
                break;
        }

    }

}
