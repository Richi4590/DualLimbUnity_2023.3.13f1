using DitzelGames.FastIK;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerControlAndPhysicsPickUp : MonoBehaviour
{
    private bool initialized = false;

    //Hand Variables and Settings
    [SerializeField] private Camera playerCamera;
    [SerializeField] private FastIKFabric IKCalculatorHand;
    [SerializeField] private FastIKFabric IKCalculatorFoot;
    [SerializeField] private GameObject targetHand;
    [SerializeField] private GameObject targetFoot;
    [SerializeField] private PickUpCollisionEvents pickupTarget;
    private Rigidbody targetFootRB;
    private Rigidbody targetHandRB;
    private Rigidbody currentObject;

    [SerializeField] private float handMovementSpeed = 3.0f;
    [SerializeField] private float footMovementSpeed = 3.0f;
    [SerializeField] private float handRotationSpeed = 3.0f;
    Vector3 cameraUp;
    Vector3 cameraForward;
    Vector3 cameraRight;
    private float currentObjectInitialAngularDrag;

    /*
    //Body Movement Variables and Settings
    [SerializeField] private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float gravityValue = -9.81f;
    */

    // Player Input Actions
    private PlayerInput playerInput;

    //Controller values
    private Vector2 leftStick_moveInputValue;
    private Vector2 rightStick_moveInputValue;
    private bool A_GrabToggleButton = false;
    private bool rightTrigger_twistButton = false;
    private bool leftShoulder_UpDownButton = false;
    private bool rightShoulder_rotateButton = false;

    public void Initialize(FastIKFabric IKH, FastIKFabric IKF, GameObject targetH, GameObject targetF, PickUpCollisionEvents pickupT, CharacterController charController, PlayerInput pInput)
    {
        IKCalculatorHand = IKH;
        IKCalculatorFoot = IKF;
        targetHand = targetH;
        targetFoot = targetF;
        pickupTarget = pickupT;
        //controller = charController;
        playerCamera = Camera.main;

        AssignedController(pInput);

        //Hand Init
        targetHandRB = targetHand.GetComponent<Rigidbody>();
        targetFootRB = targetFoot.GetComponent<Rigidbody>();
        cameraForward = playerCamera.transform.forward;
        cameraRight = playerCamera.transform.right;
        cameraUp = playerCamera.transform.up;

        handMovementSpeed = handMovementSpeed * 10.0f;
        handRotationSpeed = handRotationSpeed * 50.0f;

        initialized = true;
    }

    private void AssignedController(PlayerInput input)
    {
        Debug.Log(input);
        InputManager.actionMapChange += OnActionMapChange;
        playerInput = input;
    }
    public void UnassignCntroller(int number) //number used as a placeholder
    {
        playerInput = null;
        InputManager.actionMapChange -= OnActionMapChange;
    }

    private void OnActionMapChange(string newActionName)
    {
        playerInput.SwitchCurrentActionMap(newActionName);
    }


    private void FixedUpdate()
    {
        if (initialized)
        {
            if (playerInput?.currentActionMap.id == InputManager.inputActions.Person.Get().id)
            {
                cameraForward = playerCamera.transform.forward;
                cameraRight = playerCamera.transform.right;
                cameraUp = playerCamera.transform.up;

                Debug.Log("Foot Logic");
                MoveFootLogic();
            }
            else if (playerInput?.currentActionMap.id == InputManager.inputActions.Hand.Get().id)
            {
                Debug.Log("HandLogic");
                MoveAndRotateHandLogic();
                DoGrabLogic();
            }
        }
    }

    private IEnumerator GoTowardsHand()
    {
        yield return new WaitForFixedUpdate();
        Debug.Log(currentObject.transform.localPosition);

        currentObject.velocity = Vector3.zero;
        currentObject.transform.SetParent(pickupTarget.transform, true);
    }

    private void DoGrabLogic()
    {
        if (!pickupTarget.ObjectInPickUpRange) 
        {
            A_GrabToggleButton = false;
        }


        if (A_GrabToggleButton)
        {
            if (!currentObject)
            {
                if (pickupTarget.ObjectInPickUpRange)
                {
                    currentObject = pickupTarget.ObjectInPickUpRange.GetComponent<Rigidbody>();
                    currentObject.useGravity = false;
                    currentObject.freezeRotation = true;
                    currentObjectInitialAngularDrag = currentObject.angularDrag;
                    currentObject.angularDrag = 0;

                    StartCoroutine(GoTowardsHand());
                }
            }
            else
            {
                if (!IKCalculatorHand.stretchedToMax)
                    currentObject.velocity = targetHandRB.velocity;
                else
                    currentObject.velocity = Vector3.zero;
                //Eventuell Velocity höher machen damit man sachen wegwerfen kann?
            }

        }
        else // button not held
        {
            if (currentObject) // button not held so object should not stick
            {
                currentObject.useGravity = true;
                currentObject.freezeRotation = false;
                currentObject.angularDrag = currentObjectInitialAngularDrag;
                currentObject.transform.SetParent(null, true);
                currentObject.velocity = targetHandRB.velocity;
                currentObject = null;

                return;
            }
        }

    }

    private void MoveFootLogic()
    {
        Vector3 rightStickMoveDirection = (cameraForward.normalized * rightStick_moveInputValue.y + cameraRight.normalized * rightStick_moveInputValue.x).normalized;
        Vector3 leftStickMoveDirection = (cameraForward.normalized * leftStick_moveInputValue.y + cameraRight.normalized * leftStick_moveInputValue.x).normalized;

        /*
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        controller.Move(leftStickMoveDirection * Time.deltaTime * playerSpeed);

        if (leftStickMoveDirection != Vector3.zero)
        {
            gameObject.transform.forward = leftStickMoveDirection;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        */

        if (!IKCalculatorFoot.stretchedToMax)
        {
            Vector3 result = new Vector3(leftStickMoveDirection.x, 0.01f, leftStickMoveDirection.z) * footMovementSpeed * Time.deltaTime;
            targetFootRB.AddForce(result, ForceMode.Impulse);
        }
        else 
            targetFootRB.velocity = Vector3.zero;
    }

    private void MoveAndRotateHandLogic()
    {
        Vector3 rightStickMoveDirection = (cameraForward.normalized * rightStick_moveInputValue.y + cameraRight.normalized * rightStick_moveInputValue.x).normalized;
        Vector3 leftStickMoveDirection = (cameraForward.normalized * leftStick_moveInputValue.y + cameraRight.normalized * leftStick_moveInputValue.x).normalized;

        targetHandRB.velocity = Vector3.zero;

        if (rightShoulder_rotateButton) // rotate hand
        {
            float angleAmountX = rightStick_moveInputValue.x * handRotationSpeed * Time.deltaTime;
            float angleAmountY = -rightStick_moveInputValue.y * handRotationSpeed * Time.deltaTime;
            targetHand.transform.Rotate(Vector3.up, angleAmountX);
            targetHand.transform.Rotate(Vector3.right, angleAmountY);
        }
        else if (rightTrigger_twistButton) // twist hand
        {
            // only X Axis of right stick counts as input
            float angleAmount = rightStick_moveInputValue.x * handRotationSpeed * Time.deltaTime;
            targetHand.transform.Rotate(Vector3.forward, angleAmount);

        }
        else if (leftShoulder_UpDownButton) // only move up and down
        {
            Vector3 result = cameraUp.normalized * -leftStickMoveDirection.y * handMovementSpeed * Time.deltaTime;
            targetHandRB.velocity = result;
        }
        else // normal X and Z plane movement
        {
                Vector3 result = new Vector3(leftStickMoveDirection.x, targetHandRB.velocity.y, leftStickMoveDirection.z) * handMovementSpeed * Time.deltaTime;
                targetHandRB.velocity = result;
        }

    }


    // Controller Events
    private void OnMoveUDButton(InputValue value)
    {
        leftShoulder_UpDownButton = value.isPressed;
    }

    private void OnRotateButton(InputValue value)
    {
        rightShoulder_rotateButton = value.isPressed;
    }

    private void OnTwistHandButton(InputValue value)
    {
        rightTrigger_twistButton = value.isPressed;
    }

    private void OnGrabPressed(InputValue value)
    {
        A_GrabToggleButton = !A_GrabToggleButton;
    }

    private void OnLeftStickInput(InputValue value)
    {
        leftStick_moveInputValue = value.Get<Vector2>();
    }
    private void OnRightStickInput(InputValue value)
    {
        rightStick_moveInputValue = value.Get<Vector2>();
    }




}
