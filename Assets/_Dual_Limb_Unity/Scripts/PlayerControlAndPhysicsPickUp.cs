using DitzelGames.FastIK;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlAndPhysicsPickUp : MonoBehaviour
{
    //Hand Variables and Settings
    [SerializeField] private Camera playerCamera;
    [SerializeField] private FastIKFabric IKCalculator;
    [SerializeField] private GameObject targetHand;
    [SerializeField] private PickUpCollisionEvents pickupTarget;
    private Rigidbody targetHandRB;
    private Rigidbody currentObject;

    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float rotationSpeed = 3.0f;
    Vector3 cameraUp;
    Vector3 cameraForward;
    Vector3 cameraRight;
    private float currentObjectInitialAngularDrag;

    //Body Movement Variables and Settings
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float gravityValue = -9.81f;

    // Player Input Actions
    private PlayerInput playerInput;
    [SerializeField] private GameObject playerMeshGameobject;

    //Controller values
    private Vector2 leftStick_moveInputValue;
    private Vector2 rightStick_moveInputValue;
    private bool A_GrabToggleButton = false;
    private bool rightTrigger_twistButton = false;
    private bool leftShoulder_UpDownButton = false;
    private bool rightShoulder_rotateButton = false;

    //Mesh and Collider variables
    SkinnedMeshRenderer meshRenderer;
    MeshCollider meshCollider;

    private void OnActionMapChange(string newActionName)
    {
        playerInput.SwitchCurrentActionMap(newActionName);
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        InputManager.actionMapChange += OnActionMapChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Body Init
        controller = gameObject.GetComponent<CharacterController>();

        //Hand Init

        meshRenderer = playerMeshGameobject.GetComponent<SkinnedMeshRenderer>();
        meshCollider = playerMeshGameobject.GetComponent<MeshCollider>();

        targetHandRB = targetHand.GetComponent<Rigidbody>();
        cameraForward = playerCamera.transform.forward;
        cameraRight = playerCamera.transform.right;
        cameraUp = playerCamera.transform.up;

        movementSpeed = movementSpeed * 10.0f;
        rotationSpeed = rotationSpeed * 50.0f;
    }


    private void FixedUpdate()
    {
        UpdateCollider();
        if (playerInput.currentActionMap.id == InputManager.inputActions.Person.Get().id)
        {
            Debug.Log("BodyLogic");
            MoveBodyLogic();
        }
        else if (playerInput.currentActionMap.id == InputManager.inputActions.Hand.Get().id)
        {
            Debug.Log("HandLogic");
            MoveAndRotateHandLogic();
            DoGrabLogic();
        }
    }

    public void UpdateCollider()
    {
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);

        // Get the scale of the parent object
        Vector3 parentScale = transform.lossyScale;

        int decimalPlaces = 4;
        parentScale = new Vector3((float)System.Math.Round(1/parentScale.x, decimalPlaces), (float)System.Math.Round(1 / parentScale.y, decimalPlaces), (float)System.Math.Round(1 / parentScale.z, decimalPlaces));

        // Apply the parent's scale to the collider mesh
        Vector3[] vertices = colliderMesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Vector3.Scale(vertices[i], parentScale);
        }
        colliderMesh.vertices = vertices;

        // Set the scaled mesh to the collider
        meshCollider.sharedMesh = colliderMesh;
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
                if (!IKCalculator.stretchedToMax)
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

    private void MoveBodyLogic()
    {
        Vector3 rightStickMoveDirection = (cameraForward.normalized * rightStick_moveInputValue.y + cameraRight.normalized * rightStick_moveInputValue.x).normalized;
        Vector3 leftStickMoveDirection = (cameraForward.normalized * leftStick_moveInputValue.y + cameraRight.normalized * leftStick_moveInputValue.x).normalized;


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
    }

    private void MoveAndRotateHandLogic()
    {
        Vector3 rightStickMoveDirection = (cameraForward.normalized * rightStick_moveInputValue.y + cameraRight.normalized * rightStick_moveInputValue.x).normalized;
        Vector3 leftStickMoveDirection = (cameraForward.normalized * leftStick_moveInputValue.y + cameraRight.normalized * leftStick_moveInputValue.x).normalized;

        targetHandRB.velocity = Vector3.zero;

        if (rightShoulder_rotateButton) // rotate hand
        {
            float angleAmountX = rightStick_moveInputValue.x * rotationSpeed * Time.deltaTime;
            float angleAmountY = -rightStick_moveInputValue.y * rotationSpeed * Time.deltaTime;
            targetHand.transform.Rotate(Vector3.up, angleAmountX);
            targetHand.transform.Rotate(Vector3.right, angleAmountY);
        }
        else if (rightTrigger_twistButton) // twist hand
        {
            // only X Axis of right stick counts as input
            float angleAmount = rightStick_moveInputValue.x * rotationSpeed * Time.deltaTime;
            targetHand.transform.Rotate(Vector3.forward, angleAmount);

        }
        else if (leftShoulder_UpDownButton) // only move up and down
        {
            Vector3 result = cameraUp.normalized * -leftStickMoveDirection.y * movementSpeed * Time.deltaTime;
            targetHandRB.velocity = result;
        }
        else // normal X and Z plane movement
        {
                Vector3 result = new Vector3(leftStickMoveDirection.x, targetHandRB.velocity.y, leftStickMoveDirection.z) * movementSpeed * Time.deltaTime;
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
