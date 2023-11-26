using DitzelGames.FastIK;
using EazyCamera;
using EazyCamera.Legacy;
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

    [SerializeField] private float handMovementSpeed; //2.0f
    [SerializeField] private float footMovementSpeed; //3.0f
    [SerializeField] private float footUpwardsForce; //0.02f
    [SerializeField] private float handRotationSpeed; //3.0f
    [SerializeField] private float handMovementSpeedDuringGrab; //0.5f
    [SerializeField] private float thirdPersonCameraSpeed = 2.0f; //3.0f

    private Vector3 lastLocalPositionOfImportantItem;
    private bool importantItemHeld = false;

    Vector3 cameraUp;
    Vector3 cameraForward;
    Vector3 cameraRight;
    private float currentObjectInitialAngularDrag;

    private Rigidbody targetFootRB;
    private Rigidbody targetHandRB;
    private Rigidbody currentObject;
    private EazyCam ezCam;

    // Player Input Actions
    private PlayerInput playerInput;

    //Controller values
    private Vector2 leftStick_moveInputValue;
    private Vector2 rightStick_moveInputValue;
    private bool A_GrabToggleButton = false;
    private bool rightTrigger_twistButton = false;
    private bool leftShoulder_UpDownButton = false;
    private bool rightShoulder_rotateButton = false;

    public void Initialize(FastIKFabric IKH, FastIKFabric IKF, GameObject targetH, GameObject targetF, PickUpCollisionEvents pickupT, PlayerInput pInput)
    {
        IKCalculatorHand = IKH;
        IKCalculatorFoot = IKF;
        targetHand = targetH;
        targetFoot = targetF;
        pickupTarget = pickupT;
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
        handMovementSpeedDuringGrab = handMovementSpeedDuringGrab * 10.0f;

        if (playerCamera.TryGetComponent<EazyCam>(out EazyCam cam))
            ezCam = cam;
        else
            ezCam = null;

        initialized = true;
    }



    private void AssignedController(PlayerInput input)
    {
        Debug.Log(input);
        InputManager.actionMapChange += OnActionMapChange;
        InputManager.cameraChange += CameraChange;
        playerInput = input;
    }

    public void UnassignCntroller(int number) //number used as a placeholder
    {
        playerInput = null;
        InputManager.actionMapChange -= OnActionMapChange;
        InputManager.cameraChange -= CameraChange;
    }

    private void CameraChange(Camera newCamera)
    {
        playerCamera = newCamera;
    }

    private void OnActionMapChange(string newActionName)
    {
        playerInput.SwitchCurrentActionMap(newActionName);

        if (playerInput?.currentActionMap.id == InputManager.inputActions.Hand.Get().id)
        {
            IKCalculatorHand.enabled = true;
            IKCalculatorFoot.enabled = false;
        }
        else
        {
            //DropItemFromHand();
            IKCalculatorHand.enabled = false;
            IKCalculatorFoot.enabled = true;
        }

        UpdateCameraVectors();
    }
    private void MoveThirdPersonCamera()
    {
        float xCamSpeed = rightStick_moveInputValue.x * thirdPersonCameraSpeed;
        float yCamSpeed = rightStick_moveInputValue.y * thirdPersonCameraSpeed;

        ezCam.IncreaseRotation(xCamSpeed, yCamSpeed, Time.deltaTime);
    }

    private void UpdateCameraVectors()
    {
        cameraForward = playerCamera.transform.forward;
        cameraRight = playerCamera.transform.right;
        cameraUp = playerCamera.transform.up;
    }

    private void FixedUpdate()
    {
        if (initialized)
        {
            if (playerInput?.currentActionMap.id == InputManager.inputActions.Person.Get().id)
            {
                MoveThirdPersonCamera();
                UpdateCameraVectors();
                MoveFootLogic();
                IKCalculatorHand.MoveTargetPickupToHand(); //target collider

                if (currentObject != null) 
                    currentObject.transform.localPosition = lastLocalPositionOfImportantItem; ;
            }
            else if (playerInput?.currentActionMap.id == InputManager.inputActions.Hand.Get().id)
            {
                //Debug.Log("HandLogic");
                MoveAndRotateHandLogic();
                DoGrabLogic();
            }
        }
    }

    public void NoImportantItemHeldAnymore()
    {
        importantItemHeld = false;
        DropObjectsFromHand();
    }

    public void ParentAndGrabImportantFallenObjectAgain(Rigidbody rbObj, Vector3 lastLocalPosition, Quaternion lastLocalRotation)
    {
        currentObject = rbObj;
        A_GrabToggleButton = true;
        importantItemHeld = true;

        currentObject.velocity = Vector3.zero;
        currentObject.transform.SetParent(pickupTarget.transform, true);
        currentObject.transform.localPosition = lastLocalPosition;
        currentObject.transform.rotation = lastLocalRotation;
        lastLocalPositionOfImportantItem = lastLocalPosition;


        currentObject.useGravity = false;
        currentObject.freezeRotation = true;
        currentObjectInitialAngularDrag = currentObject.angularDrag;
        currentObject.angularDrag = 0;
    }

    private void DropObjectsFromHand()
    {
        if (currentObject != null)
        {
            currentObject.useGravity = true;
            currentObject.freezeRotation = false;
            currentObject.angularDrag = currentObjectInitialAngularDrag;
            currentObject.transform.SetParent(null, true);
            currentObject.velocity = targetHandRB.velocity;
            currentObject = null;
        }
    }

    private IEnumerator GoTowardsHand()
    {
        yield return new WaitForFixedUpdate();

        currentObject.velocity = Vector3.zero;
        currentObject.transform.SetParent(pickupTarget.transform, true);
        
        if (currentObject.TryGetComponent<LastHandGrabbed>(out LastHandGrabbed lhg))
        {
            lhg.lastPlayerController = this;
            lhg.lastValidGrabbedLocalPosition = currentObject.transform.localPosition;
            lhg.lastValidGrabbedRotation = currentObject.transform.rotation;

            lastLocalPositionOfImportantItem = currentObject.transform.localPosition;   
        }
    }

    private void DoGrabLogic()
    {
        if (!pickupTarget.ObjectInPickUpRange) 
        {
            A_GrabToggleButton = false;
        }
        

        if (A_GrabToggleButton || importantItemHeld)
        {
            if (!currentObject)
            {
                if (pickupTarget.ObjectInPickUpRange)
                {
                    if (pickupTarget.ObjectInPickUpRange.TryGetComponent<Rigidbody>(out Rigidbody rb))
                    {
                        currentObject = rb;
                    }
                    else
                    {
                        Transform parent = pickupTarget.ObjectInPickUpRange.transform.parent;
                        currentObject = parent.GetComponent<Rigidbody>();
                    }
                   
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
        Vector3 leftStickMoveDirection = (cameraForward.normalized * leftStick_moveInputValue.y + cameraRight.normalized * leftStick_moveInputValue.x).normalized;

        if (!IKCalculatorFoot.stretchedToMax && (leftStick_moveInputValue.x > 0f || leftStick_moveInputValue.x < 0f  || 
                                                 leftStick_moveInputValue.y < 0f || leftStick_moveInputValue.y > 0))
        {
            Vector3 result = new Vector3(leftStickMoveDirection.x, footUpwardsForce, leftStickMoveDirection.z) * footMovementSpeed * Time.deltaTime;
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
        Vector3 result;

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
            if (currentObject == null)
            {
                result = cameraUp.normalized * -leftStickMoveDirection.y * handMovementSpeed * Time.deltaTime;
                //targetHandRB.AddForce(result, ForceMode.Impulse);
            }
            else
            {
                result = cameraUp.normalized * -leftStickMoveDirection.y * handMovementSpeedDuringGrab * Time.deltaTime;
                //targetHandRB.AddForce(result, ForceMode.Impulse);
            }


            targetHandRB.velocity = result;
        }
        else // normal X and Z plane movement
        {
            if (currentObject == null)
            {
                result = new Vector3(leftStickMoveDirection.x, targetHandRB.velocity.y, leftStickMoveDirection.z) * handMovementSpeed * Time.deltaTime;
                //targetHandRB.AddForce(result, ForceMode.Impulse);

            }
            else
            {
                result = new Vector3(leftStickMoveDirection.x, targetHandRB.velocity.y, leftStickMoveDirection.z) * handMovementSpeedDuringGrab * Time.deltaTime;
                //targetHandRB.AddForce(result, ForceMode.Impulse);
            }

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
