using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Velocidades M�ximas")]
    [SerializeField] private float maxUpSpeed = 5f;  // M�x velocidad vertical (hacia arriba)
    [SerializeField] private float maxMovementSpeed = 5f;  // M�x velocidad horizontal

    [Header("Constantes de Aceleraci�n")]
    [SerializeField] private float upAccelerationConstant = 1f;
    [SerializeField] private float inputMovementAccelerationConstant = 5f;
    [SerializeField] private float inputMovementDecelerationConstant = 5f;

    [Header("Bounciness")]
    [SerializeField] private float bounceMultiplier = 2f;  // Extra �oomph� on reflection

    // Debug info
    private float _currentUpSpeed;
    private float _currentMovementSpeed;

    private Rigidbody _rb;


    private InputAction _moveAction;
    private Vector2 _moveInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        // If the player is meant to float up continuously, you might want to disable gravity:
        // _rb.useGravity = false;

        // For fewer tunneling issues at higher speeds, enable continuous collision detection
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // If you don't want the player to tilt or roll, freeze rotation on certain axes:
        // _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        _moveAction = GetComponent<PlayerInput>().actions["Move"];

        var player = GetComponent<PlayerInput>().playerIndex;

    }

    private void OnGUI()
    {
        var style = new GUIStyle { fontSize = 20 };
        GUILayout.Label($"Current Up Speed: {_currentUpSpeed}", style);
        GUILayout.Label($"Current Movement Speed: {_currentMovementSpeed}", style);
        GUILayout.Label($"Move Input: {_moveInput}", style);
        GUILayout.Label($"Rigidbody Velocity: {_rb.linearVelocity}", style);
    }

    private void Update()
    {
        // Cache input each frame in Update (for responsiveness)
        _moveInput = _moveAction.ReadValue<Vector2>();
       
    }

    private void FixedUpdate()
    {

        // Read current velocity
        Vector3 currentVelocity = _rb.linearVelocity;

        // ------------------------
        // 1) Apply upward acceleration (if you want the player to move up until maxUpSpeed)
        // ------------------------
        if (currentVelocity.y < maxUpSpeed)
        {
            // Add upward force (Acceleration mode modifies velocity ignoring mass, each FixedUpdate)
            _rb.AddForce(Vector3.up * upAccelerationConstant, ForceMode.Acceleration);
        }

        // ------------------------
        // 2) Horizontal movement
        // ------------------------
        Vector3 horizontalVel = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
        bool isMoving = _moveInput.sqrMagnitude > 0.0001f;

        if (isMoving)
        {
            // Desired direction from input
            Vector3 desiredDir = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;
            // Our target velocity in that direction
            Vector3 desiredHorizontalVel = desiredDir * maxMovementSpeed;

            // The difference from current horizontal velocity
            Vector3 diff = desiredHorizontalVel - horizontalVel;
            // Add force to approach that velocity
            Vector3 force = diff * inputMovementAccelerationConstant;
            _rb.AddForce(force, ForceMode.Acceleration);
        }
        else
        {
            // No input? Decelerate horizontally toward zero
            Vector3 diff = Vector3.zero - horizontalVel;
            Vector3 force = diff * inputMovementDecelerationConstant;
            _rb.AddForce(force, ForceMode.Acceleration);
        }

        // ------------------------
        // 3) (Optional) Final velocity clamp
        // ------------------------
        // NOTE: If you clamp below the bounce velocity, you lose some �bounce effect.�
        //       If you WANT the bounce to exceed normal speeds, skip or partially skip the clamp.
        currentVelocity = _rb.linearVelocity;

        // a) Limit horizontal speed to maxMovementSpeed (if desired)
        //Vector3 newHoriz = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
        //float hSpeed = newHoriz.magnitude;
        //if (hSpeed > maxMovementSpeed)
        //{
        //    // If you truly do NOT want to exceed maxMovementSpeed even after bouncing:
        //    // normalize horizontal and keep vertical the same
        //    newHoriz = newHoriz.normalized * maxMovementSpeed;
        //    currentVelocity.x = newHoriz.x;
        //    currentVelocity.z = newHoriz.z;
        //}

        // b) Limit upward speed to maxUpSpeed (again, you can skip if you want bounce above maxUpSpeed)
        if (currentVelocity.y > maxUpSpeed)
        {
            currentVelocity.y = maxUpSpeed;
        }

        // Reassign if changed
        _rb.linearVelocity = currentVelocity;

        // For debug
        _currentUpSpeed = currentVelocity.y;
        _currentMovementSpeed = new Vector2(currentVelocity.x, currentVelocity.z).magnitude;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 1) Current velocity
        Vector3 v = _rb.linearVelocity;

        // 2) Get collision normal
        Vector3 n = collision.contacts[0].normal.normalized;

        Vector3 normal = collision.contacts[0].normal;

        Debug.DrawLine(collision.contacts[0].point, normal,Color.red,15f);


        float dot = Vector3.Dot(_rb.linearVelocity, normal);
        if (dot >= 0) return; // now you only bounce if dot < 0

        // 3) Break velocity into components normal and tangential to n
        float normalDot = Vector3.Dot(v, n);
        Vector3 normalComponent = normalDot * n;
        Vector3 tangential = v - normalComponent;

        // 4) Flip the normal component, multiply by bounce factor (coefficient of restitution)
        Vector3 newNormalComponent = -normalComponent * bounceMultiplier;

        // 5) Rebuild new velocity
        Vector3 newVelocity = tangential + newNormalComponent;

        // 6) Add the delta
        Vector3 velocityChange = newVelocity - v;

        _rb.linearVelocity = Vector3.zero;

        _rb.AddForce(velocityChange, ForceMode.VelocityChange);

        Debug.Log("COLLISION");
    }

}
