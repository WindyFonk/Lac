using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFaceTowardCam : MonoBehaviour
{
    public float _scaleFactor = 0.1f;
    public Camera _mainCamera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
        Scale();
    }


    private void Scale()
    {
        if (_mainCamera)
        {
            float camHeight;
            if (_mainCamera.orthographic)
            {
                camHeight = _mainCamera.orthographicSize * 2;
            }
            else
            {
                float distanceToCamera = Vector3.Distance(_mainCamera.transform.position, transform.position);
                camHeight = 2.0f * distanceToCamera * Mathf.Tan(Mathf.Deg2Rad * (_mainCamera.fieldOfView * 0.5f));
            }
            float scale = (camHeight / Screen.width) * _scaleFactor;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
