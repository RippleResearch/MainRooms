using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MirrorCameraScript : MonoBehaviour
{
    public Transform target;
    private Camera cam;
    private CameraSwitchController cameraSwitchController;
    private TouchController touchController;
    public float zoomSpeed = 15;

    //Default Settings
    private Vector3 defaultOffset;
    public Vector3 dynamicOffset;  //Offset to remeber change when user rotates camera
    private Quaternion defaultRotation;
    private float defaultFOV;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cameraSwitchController = GetComponentInParent<CameraSwitchController>();
        touchController = GetComponentInParent<TouchController>();
        //tests
        Debug.Assert(cameraSwitchController != null);
        Debug.Assert(cam != null);
        Debug.Assert(touchController != null);

        //Defaults
        dynamicOffset = defaultOffset = new Vector3(-6, 4, 0);
        defaultRotation = cam.transform.rotation;
        defaultFOV = 60;
    }

    // Update is called once per frame
    private void Update()
    {
        //Follow the target
        cameraSwitchController.follow(cam, target, dynamicOffset, 0.125f);

        if (cam.enabled)
        {
            //if double touch reset camera
            if (touchController.listenDoubleTouch())
            {
                Debug.Log("Resetting Camera");
                dynamicOffset = defaultOffset;
                cameraSwitchController.ResetFOV("MainMirrorCamera",  defaultFOV);
            }

            //Change camera zoom (Assuming you are using main camera)
           cam.fieldOfView = cameraSwitchController.keepInRange(cam.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, 35, 90);

        }

    }

    
}
