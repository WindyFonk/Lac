using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class ThrowObjectFPS : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] float checkDistance;
    [SerializeField] LayerMask objectLayer;
    RaycastHit hit;

    private GameObject holdingObject;
    private GameObject _object;

    private bool isHolding;
    private bool isLooking;

    [SerializeField] GameObject pickUpUI;
    [SerializeField] Transform pickUpRoot;

    [SerializeField] float throwForce;
    [SerializeField] float throwForceUp;

    MakeSound objectSound;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ScanObject();
        pickUI();

        PickUpObject();
        DropObject();
        ThrowObject();
    }

    private void pickUI()
    {
        if (isLooking && !isHolding)
        {
            pickUpUI.SetActive(true);
        }
        else
        {
            pickUpUI.SetActive(false);
        }
    }
    private void ScanObject()
    {
        if (isHolding) return;

        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, checkDistance, objectLayer))
        {
            _object = hit.collider.gameObject;
            isLooking = true;
        }
        else
        {
            _object = null;
            isLooking = false;
        }
    }

    private void PickUpObject()
    {
        if (!_object) return;
        if (isHolding) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            objectSound = _object.GetComponent<MakeSound>();
            holdingObject = _object;
            holdingObject.transform.SetParent(pickUpRoot, false);
            holdingObject.GetComponent<Collider>().enabled = false;
            holdingObject.GetComponent<Rigidbody>().isKinematic = true;
            isHolding = true;
            holdingObject.transform.position = pickUpRoot.position;
        }
    }

    private void DropObject()
    {
        if (!isHolding) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            holdingObject.GetComponent<Collider>().enabled = true;
            holdingObject.transform.SetParent(null);
            holdingObject.GetComponent<Rigidbody>().isKinematic = false;
            isHolding = false;
            holdingObject = null;
        }
    }

    private void ThrowObject()
    {
        if (!isHolding) return;

        if (Input.GetMouseButtonDown(0))
        {
            holdingObject.GetComponent<Collider>().enabled = true;
            holdingObject.transform.SetParent(null);
            holdingObject.GetComponent<Rigidbody>().isKinematic = false;
            objectSound.canMakeSound = true;


            Vector3 force = _camera.transform.forward * throwForce + transform.up * throwForceUp;
            holdingObject.GetComponent<Rigidbody>().AddForce(force);


            isHolding = false;
            holdingObject = null;
        }
    }


}
