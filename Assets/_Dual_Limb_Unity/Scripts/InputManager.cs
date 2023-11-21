using DitzelGames.FastIK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionAsset asset;
    public static LimbInputSystem inputActions;
    public static event Action<string> actionMapChange;
    public static event Action<Camera> cameraChange;
    private static Guid currentInputActionId;

    [SerializeField] private GameObject PlayerInputsRoot;
    private GameObject Left_PlayerInputObj;
    private GameObject Right_PlayerInputObj;

    [SerializeField] private Camera thirdPersonCamera;
    [SerializeField] private CharacterController characterController;

    //Left Player Settings
    [SerializeField] private FastIKFabric LeftIKFabricHand;
    [SerializeField] private FastIKFabric LeftIKFabricFoot;
    [SerializeField] private GameObject targetHandLeft;
    [SerializeField] private GameObject targetFootLeft;
    [SerializeField] private PickUpCollisionEvents pickupTargetLeft;
    private PlayerInput Left_PlayerInput;

    //Right Player Settings
    [SerializeField] private FastIKFabric RightIKFabricHand;
    [SerializeField] private FastIKFabric RightIKFabricFoot;
    [SerializeField] private GameObject targetHandRight;
    [SerializeField] private GameObject targetFootRight;
    [SerializeField] private PickUpCollisionEvents pickupTargetRight;
    private PlayerInput Right_PlayerInput;


    private int playersJoined = 0;

    private void Awake()
    {
        inputActions = new LimbInputSystem();
        inputActions.Disable();
        currentInputActionId = inputActions.Person.Get().id;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public static void ToggleActionMap(InputActionMap actionMap)
    {
        currentInputActionId = actionMap.id;
        actionMapChange?.Invoke(actionMap.name);
    }

    public static void ToggleCameraChange(Camera camera)
    {
        cameraChange?.Invoke(camera);
    }

    public static void TogglePersonControlScheme()
    {
        currentInputActionId = inputActions.Person.Get().id;
        actionMapChange?.Invoke(inputActions.Person.Get().name);
    }

    public static void ToggleHandControlScheme()
    {
        currentInputActionId = inputActions.Hand.Get().id;
        actionMapChange?.Invoke(inputActions.Hand.Get().name);
    }

    public void PlayerJoinedEvent(PlayerInput input)
    {
        input.camera = Camera.main;

        if (playersJoined == 0)
        {
            Left_PlayerInputObj = input.gameObject;
            input.gameObject.transform.SetParent(PlayerInputsRoot.transform);

            Left_PlayerInputObj.GetComponent<PlayerControlAndPhysicsPickUp>()
                .Initialize(LeftIKFabricHand, LeftIKFabricFoot, targetHandLeft, targetFootLeft, pickupTargetLeft, characterController, input);
        }
        else
        {
            Right_PlayerInputObj = input.gameObject;
            input.gameObject.transform.SetParent(PlayerInputsRoot.transform);

            Right_PlayerInputObj.GetComponent<PlayerControlAndPhysicsPickUp>()
                .Initialize(RightIKFabricHand, RightIKFabricFoot, targetHandRight, targetFootRight, pickupTargetRight, characterController, input);
        }

        if (currentInputActionId == inputActions.Person.Get().id)
        {
            ToggleActionMap(inputActions.Person);
        }
        else if (currentInputActionId == inputActions.Hand.Get().id)
        {
            ToggleActionMap(inputActions.Hand);

        }

        Debug.Log("Controller joined");
        input.ActivateInput();
        playersJoined++;
    }

    public void PlayerLeftEvent(PlayerInput input)
    {
        Debug.Log("Controller disconnected");

        if (Left_PlayerInput == input)
        {
            Left_PlayerInput.DeactivateInput();
            Left_PlayerInputObj.GetComponent<PlayerControlAndPhysicsPickUp>().UnassignCntroller(0);
        }
        else
        {
            Right_PlayerInput.DeactivateInput();
            Right_PlayerInputObj.GetComponent<PlayerControlAndPhysicsPickUp>().UnassignCntroller(0);
        }

        Destroy(input.gameObject);

        playersJoined--;
    }
}
