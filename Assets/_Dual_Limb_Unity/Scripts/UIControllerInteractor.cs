using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIControllerInteractor : MonoBehaviour
{
    [SerializeField] GameObject MainCanvasRoot;
    [SerializeField] GameObject SettingsMenuRoot;

    [SerializeField] Selectable firstButtonMainCanvas;
    [SerializeField] Selectable firstButtonSettingsCanvas;
    [SerializeField] Selectable firstButtonPlayCanvas;

    private void Update()
    {
        // Check for navigation input (e.g., arrow keys or controller dpad)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Check if there's any input
        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Get the currently selected UI element
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

            if (selectedObject == null)
            {
                if (SettingsMenuRoot.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(firstButtonSettingsCanvas.gameObject);
                }
                else if (MainCanvasRoot.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(firstButtonMainCanvas.gameObject);
                }
                else //Play Screen
                {
                    EventSystem.current.SetSelectedGameObject(firstButtonPlayCanvas.gameObject);
                }



            }
        }
    }
}
