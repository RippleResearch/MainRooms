using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


public class MainCameraScript : MonoBehaviour
{
    //Information that is public is found in the editor
    public Transform target; //Get information about our target (the ship in this case)
    private Camera cam;

    public float smoothTime = 0.125F;
    public Vector3 defaultOffset; //For distance between the camera and the character (Ship in this case), default is recommended offset
    public Quaternion defaultRotation;  // Remeber default rotation
    public Vector3 dynamicOffset;  //Offset to remeber change when user rotates camera

    //Camera zoom
    public float zoomSpeed = 15;
    public float touchZoomSpeed = .5f;
  
    //Camera Rotate
    public float rotateSpeed = 10;
    private Vector2 turn; //Set so first rotation will keep default and not set to zero


    //Camera Selector Componenet
    private CameraSwitchController cameraSwitchController;
    private TouchController touchController;


    private Vector3 velocity = Vector3.zero; //Ref need according to unity docs for smooth damp

    public void Start()
    {
        cameraSwitchController = GetComponentInParent<CameraSwitchController>();
        touchController = GetComponentInParent<TouchController>();
        cam = GetComponent<Camera>();

        //tests
        Debug.Assert(cameraSwitchController != null);
        Debug.Assert(touchController != null);

        //defaults
        dynamicOffset = defaultOffset = new Vector3(16, 7, -3);
        defaultRotation = Camera.main.transform.rotation;
        turn = new Vector2(-97, 97);
    }

    /* Every Frame we get the position we want to attach to. 
     * We then use SmoothDamp to smooth our camera follow over time. How much closer we get
     * depends on smooth speed based on frame. Smooth speed is just an arbitrary time, the larger it is
     * the longer it takes for the camera to reach its location. 
     */
    private void Update()
    {
        //Follow the target
        cameraSwitchController.follow(cam, target, dynamicOffset, smoothTime);

        if (cam.enabled)
        { 
            //Double mouse click Rotation Change
            //changeRotationTouch();

            //if double touch reset camera
            if (touchController.listenDoubleTouch()) 
            {
                Debug.Log("Resetting Camera");
                dynamicOffset = defaultOffset;
                cameraSwitchController.resetCamera("MainCamera", defaultOffset, defaultRotation, 27.9f); 
            }

            //Change camera zoom (Assuming you are using main camera)
            cam.fieldOfView = cameraSwitchController.keepInRange(cam.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, 10, 75);
           
        }
        
    }

    //change to be generic with parameters
    private void follow()
    {
        Vector3 desiredPosition = target.position + dynamicOffset; //Automatically uses vector math
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime); //Smooths to position over a given time
    }

    private void changeRotationTouch()
    {
            Debug.Log("Changing Rotation");
            transform.LookAt(target);
            //dynamicOffset += Vector3.right;
            transform.Translate(Vector3.right * Time.deltaTime);
    }

   
    private void changeRotationMouse()
    {
        //Check for double click
        //If found change rotation but keep a given offset no matter what
        //In order to stop other scripts (like movement of the effect as well as movement of the boat)
        //In the respective scripts right mouse button is checked, if it is down they return nothing and assume that rotation is taking place
        if(Input.GetMouseButton(0) && Input.GetMouseButton(1)) 
        {
            turn.x += Input.GetAxis("Mouse X");
            turn.y -= Input.GetAxis("Mouse Y"); //Because it is default inverted we do -= to make them normal
            transform.localRotation = Quaternion.Euler(turn.y - 77, turn.x + 20, 0);  //(-77 and so serves as the offset of original camera location)
        }

        if (Input.GetMouseButton(2)) //Reset rotation
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(20, -77, 0), smoothTime);
        }
    }

    
}
