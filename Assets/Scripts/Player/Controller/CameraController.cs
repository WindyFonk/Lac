using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;

    public float rotationSpeed;
    private Rigidbody rb;


    //First Camera Controller
    private float _xRotation;
    [SerializeField] float upLimit = -40f;
    [SerializeField] float downLimit = 70f;
    [SerializeField] float mouseSensitive = 21f;

    [SerializeField] Transform _camera;
    [SerializeField] Transform _cameraRoot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        FirstCamController();

    }

    private void FirstCamController()
    {

        var Mouse_X = Input.GetAxis("Mouse X");
        var Mouse_Y = Input.GetAxis("Mouse Y");


        _xRotation -= Mouse_Y * mouseSensitive * Time.smoothDeltaTime;
        _xRotation = Mathf.Clamp(_xRotation, upLimit, downLimit);

        _camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        _camera.position = _cameraRoot.position;
    
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, Mouse_X * mouseSensitive * Time.smoothDeltaTime, 0));
    }

    private void ThirdCamController()
    {
        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
    }
}
