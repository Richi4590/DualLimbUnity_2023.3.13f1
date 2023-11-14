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

    [SerializeField] private GameObject PlayerInputsRoot;
    private GameObject Left_PlayerInputObj;
    private GameObject Right_PlayerInputObj;

    [SerializeField] private CharacterController characterController;

    //Left Player Settings
    [SerializeField] private FastIKFabric LeftIKFabricHand;
    [SerializeField] private GameObject targetLeft;
    [SerializeField] private PickUpCollisionEvents pickupTargetLeft;
    private PlayerInput Left_PlayerInput;

    //Right Player Settings
    [SerializeField] private FastIKFabric RightIKFabricHand;
    [SerializeField] private GameObject targetRight;
    [SerializeField] private PickUpCollisionEvents pickupTargetRight;
    private PlayerInput Right_PlayerInput;


    private int playersJoined = 0;

    private void Awake()
    {
        inputActions = new LimbInputSystem();
        inputActions.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        ToggleActionMap(inputActions.Hand);   
    }

    public static void ToggleActionMap(InputActionMap actionMap)
    {
        actionMapChange?.Invoke(actionMap.name);
    }

    public void PlayerJoinedEvent(PlayerInput input)
    {
        input.camera = Camera.main;

        if (playersJoined == 0)
        {
            Left_PlayerInputObj = input.gameObject;
            input.gameObject.transform.SetParent(PlayerInputsRoot.transform);

            Left_PlayerInputObj.GetComponent<PlayerControlAndPhysicsPickUp>()
                .Initialize(LeftIKFabricHand, targetLeft, pickupTargetLeft, characterController, input);
        }
        else
        {
            Right_PlayerInputObj = input.gameObject;
            input.gameObject.transform.SetParent(PlayerInputsRoot.transform);

            Right_PlayerInputObj.GetComponent<PlayerControlAndPhysicsPickUp>()
                .Initialize(RightIKFabricHand, targetRight, pickupTargetRight, characterController, input);
        }

        Debug.Log("Controller joined");
        ToggleActionMap(inputActions.Hand);
        input.ActivateInput();
        playersJoined++;
    }

    /*
         public void PlayerJoinedEvent(PlayerInput input)
    {

        if (playersJoined == 0)
        {
            Left_PlayerInput = PlayerInput.Instantiate(Left_PlayerInputObj, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[playersJoined]);
            Left_PlayerInput.actions = asset;
            Left_PlayerInput.camera = Camera.main;
            Left_PlayerInput.notificationBehavior = PlayerNotifications.SendMessages;
            Left_PlayerInput.ActivateInput();
            Left_PlayerInputObj.GetComponent<PlayerControlAndPhysicsPickUp>().AssignedController(Left_PlayerInput);
        }
        else
        {
            Right_PlayerInput = PlayerInput.Instantiate(Right_PlayerInputObj, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[playersJoined]); ;
            Left_PlayerInput.actions = asset;
            Right_PlayerInput.camera = Camera.main;
            Left_PlayerInput.notificationBehavior = PlayerNotifications.SendMessages;
            Right_PlayerInput.ActivateInput();
            Right_PlayerInputObj.GetComponent<PlayerControlAndPhysicsPickUp>().AssignedController(Right_PlayerInput);
        }

        Debug.Log("Controller joined");

        playersJoined++;
    }
     
     */

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
