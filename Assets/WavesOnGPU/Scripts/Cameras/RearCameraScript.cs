using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;


/// <summary>
/// Pretty much a repeat of the little functions needed from the Camera Follow Class found in the main camera
/// Consider reducing code by making a parent class function hierarchy. But for now, it works. 
/// </summary>
public class RearCameraScript : MonoBehaviour
{

    private Camera cam;
    private CameraSwitchController cameraSwitchController;
    private TouchController touchController;
    public float zoomSpeed = 15;

    //Default Settings
    private Vector3 defaultOffset;
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
        defaultOffset = new Vector3(0.4f, 63, -53);
        defaultRotation = cameraSwitchController.getCamera("RearCamera").transform.rotation;
        defaultFOV = 74.5f;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cam.enabled)
        {
            //Change camera zoom (Assuming you are using main camera)
            cam.fieldOfView = cameraSwitchController.keepInRange(cam.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, 7, 100);
            //Reset Camera on doubleTouch
           if (touchController.listenDoubleTouch()) 
            { 
                cameraSwitchController.resetCamera("RearCamera", defaultOffset, defaultRotation, defaultFOV); 
            }
        }
    }

}
