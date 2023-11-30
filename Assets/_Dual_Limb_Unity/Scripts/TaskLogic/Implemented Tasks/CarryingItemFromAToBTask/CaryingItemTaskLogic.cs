using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CaryingItemTaskLogic : Task
{
    [SerializeField] private ItemTransportArea transportArea;
    [SerializeField] private List<GameObject> anyItemRequiredToAtLeastGrab;
    [SerializeField] private List<GameObject> listOfItemsToTransport;
    [SerializeField] private int itemsToTransport;
    [SerializeField] private GameObject placingAreaRoot;
    private GameObject importantHeldItem;

    private int itemsTransported = 0;
    private bool importantItemInHand = false;


    // Start is called before the first frame update
    void Start()
    {
        if (listOfItemsToTransport.Count > 0)
            itemsToTransport = listOfItemsToTransport.Count;

        transportArea.correctItemEntered += ItemEnteredTransportAreaEvent;
        transportArea.correctItemExited += ItemLeftTransportAreaEvent; 

    }

    private void Update()
    {
        if (!importantItemInHand)
            StartCoroutine(checkForAnyImportangItemHeld());
    }

    private IEnumerator checkForAnyImportangItemHeld()
    {
        yield return new WaitForEndOfFrame();

        foreach (GameObject g in anyItemRequiredToAtLeastGrab)
        {
            Rigidbody rb = g.GetComponent<Rigidbody>();

            if (rb.isKinematic == true) // important item grabbed
            {
                importantHeldItem = g;
                importantHeldItem.GetComponent<Collider>().enabled = false;
                importantItemInHand = true;
                GoToThirdPersonMode();
            }

        }

        yield return new WaitForEndOfFrame();
    }

    public void ItemEnteredTransportAreaEvent()
    {
        itemsTransported++;

        if (itemsToTransport == itemsTransported)
        {
            PlayerControlAndPhysicsPickUp[] controllers = GameObject.FindObjectsByType<PlayerControlAndPhysicsPickUp>(FindObjectsSortMode.None);

            for (int i = 0; i < controllers.Length; i++)
                controllers[i].NoImportantItemHeldAnymore();

            placingAreaRoot.GetComponent<Collider>().enabled = false;
            placingAreaRoot.SetActive(false);
            MarkTaskAsCompleted();
        }

    }

    public void ItemLeftTransportAreaEvent()
    {
        itemsTransported--;
    }

    public void TriggerFirstPersonTo(Collider other, PositionPersonForTask ppft)
    {
            importantHeldItem.GetComponent<Collider>().enabled = true;
            jointManager.FreezeBody(true);
            jointManager.ResetToDefaultBodyPositionAndRotation();
            InputManager.ToggleHandControlScheme();
            InputManager.ToggleCameraChange(ppft.PositioningHelperCamera);
            other.gameObject.transform.position = ppft.transform.position;
            CameraManager.Instance.SetActiveThirdPersonCamera(false);
            ppft.PositioningHelperCamera.enabled = true;
            ppft.SetHandsPosition();

            //other.gameObject.transform.parent.rotation = positioningScript.transform.rotation;
    }
}
