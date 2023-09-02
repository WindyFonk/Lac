using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    private float speed;
    public float walkspeed;
    public float runSpeed = 18f;
    public float crouchSpeed = 8f;
    public Transform orientation;
    public Transform cam;
    public bool isAiming;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    public float GroundedRadius = 0.28f;
    public float GroundedOffset = -0.14f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;


    [SerializeField] float turnSpeed = 0.1f;
    private float turnSmoothVelocity;

    float horizontal;
    float vertical;

    Rigidbody rb;
    private bool isWalking;
    public bool isCrouching;
    private bool isRunning;
    Vector3 direction;
    private GameObject _mainCamera;
    public bool Grounded;

    private bool canControl = true;
    private bool canStandUp = true;


    [SerializeField] Collider standingCollider;
    [SerializeField] Collider crouchingCollider;
    [SerializeField] Transform headPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Move()
    {
        horizontal = UnityEngine.Input.GetAxisRaw("Horizontal");
        vertical = UnityEngine.Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude > 0)
        {
            //Face toward moving direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSpeed);
            Vector3 forwardDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            rb.AddForce(forwardDirection.normalized * speed * 100, ForceMode.Force);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        if (UnityEngine.Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.C) && canStandUp)
        {
            isCrouching = !isCrouching;
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            isCrouching = false;
        }


        //MOving state
        if (isCrouching)
        {
            speed = crouchSpeed;
        }
        else if (isRunning)
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkspeed;
        }

        standingCollider.enabled = !isCrouching;
        crouchingCollider.enabled = isCrouching;



    }
}
