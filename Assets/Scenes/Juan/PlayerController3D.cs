using System;
using Assets.Scenes.Juan;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController3D : MonoBehaviour
{
    public PlayerInput PlayerInput { get; private set; }

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject camerasObjects;

    public Rigidbody rb;
    [SerializeField] private float maxLinearVelocity = 3f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxYSpeed = 5f;

    Vector2 moveInput;
    public Vector3 moveDirection;
    CharacterController characterController;
    private float initialVerticalSpeed = 1f;
    private float incrementalVerticalSpeedPerSecond = 0.01f;

    private float currentVerticalSpeed = 0;
    InputAction actionMap;

    public Camera PlayerCamera;


    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 

        int playerIndex = PlayerInput.playerIndex;

        Cursor.lockState = CursorLockMode.Confined;
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        actionMap = PlayerInput.actions["Move"];

      //  GetComponentInChildren<InputHandler>().horizontal = actionMap;
      //  GetComponentInChildren<InputHandler>().vertical = actionMap;

        currentVerticalSpeed = initialVerticalSpeed;
        camerasObjects.transform.SetParent(null);

        camerasObjects.name = $"Cameras - Player {playerIndex}";
        gameObject.name = $"Player {playerIndex}";
    }

    private void Move()
    {
        // Get the input from the context
        moveInput = actionMap.ReadValue<Vector2>();

        // Get the camera's forward and right vectors, ignoring the Y component
        Vector3 cameraForward = PlayerCamera.transform.forward;
        cameraForward.y = 0; // Ignore vertical component
        cameraForward.Normalize(); // Normalize to maintain consistent magnitude

        Vector3 cameraRight = PlayerCamera.transform.right;
        cameraRight.y = 0; // Ignore vertical component
        cameraRight.Normalize(); // Normalize to maintain consistent magnitude

        // Calculate move direction based on input and camera orientation
        moveDirection = cameraRight * moveInput.x + cameraForward * moveInput.y;
        // Optionally normalize moveDirection to prevent faster diagonal movement
        if (moveDirection.magnitude > 1)
        {
            moveDirection.Normalize();
        }
    }

    public void LookAtCameraDirection()
    {
        var cameraRotation = PlayerCamera.transform.rotation.eulerAngles;
        cameraRotation.x = 0;
        cameraRotation.z = 0;
        transform.rotation = Quaternion.Euler(cameraRotation);
    }


    private void OnDrawGizmos()
    {
       // Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, moveDirection*5);
        //Gizmos.DrawRay(transform.position, rb.linearVelocity.normalized * 5);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.maxLinearVelocity = maxLinearVelocity;
        if (actionMap.IsPressed())
        {
            LookAtCameraDirection();
        }
        else
        {
            moveInput = Vector2.zero;
            moveDirection = Vector3.zero;
        }

        Move();

        Debug.Log(rb.linearVelocity.magnitude);

        animator.SetBool("run", rb.linearVelocity.magnitude > Vector3.one.magnitude);
        

       // moveDirection.x = moveInput.x * transform.right;

       

        if(rb.linearVelocity.y > maxYSpeed)
        {
            moveDirection.y = 0;
            currentVerticalSpeed = 0;
        }
        else
        {
            currentVerticalSpeed += incrementalVerticalSpeedPerSecond * Time.deltaTime;
            moveDirection.y = currentVerticalSpeed;

        }
        moveDirection.y = 0;

        rb.AddForce(moveDirection.normalized*moveSpeed);
        //rb.AddForce(Vector3.up * currentVerticalSpeed);
        //characterController.Move(moveDirection * Time.deltaTime* moveSpeed);
        //characterController.Move(Vector3.up * Time.deltaTime * currentVerticalSpeed);
    }
}
