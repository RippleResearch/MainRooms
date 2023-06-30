using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class serves to function as a general controller for cameras. The functions in this 
/// class are used by all cameras attached to the boat (thus exculding the orthagraphic obstacleCamera)
/// Its main functions are to caputure touch input and act accordinly. 
/// </summary>
public class CameraSwitchController : MonoBehaviour
{

    private Camera mainCam, rearCam, mainMirrorCam, curCam;
    private float touchZoomSpeed = .25f;


    //Dictionary of cameras
    Dictionary<string, Camera> string_to_camera = new Dictionary<string, Camera>();
    //Inverse map for setting camera struct
    Dictionary<Camera, Vector2> camera_to_vector2 = new Dictionary<Camera, Vector2>();

    //Ref need according to unity docs for smooth damp
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        //Get cameras  (use tag for main camera to prevent null error when disabling it)
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        rearCam = GameObject.FindWithTag("RearCam").GetComponent<Camera>();
        mainMirrorCam = GameObject.FindWithTag("MainMirrorCamera").GetComponent<Camera>();

        //Assert we have found the correct component
        Debug.Assert(mainCam != null);
        Debug.Assert(rearCam != null);
        Debug.Assert(mainMirrorCam != null);

        //set enabled cameras (Can use an array for more cameras in future)
        mainCam.enabled = true;
        rearCam.enabled = false;
        mainMirrorCam.enabled = false;

        //Add cameras to maps
        string_to_camera.Add("MainCamera", mainCam);
        camera_to_vector2.Add(mainCam, new Vector2(10, 75));

        string_to_camera.Add("RearCamera", rearCam);
        camera_to_vector2.Add(rearCam, new Vector2(15, 100));

        string_to_camera.Add("MainMirrorCamera", mainMirrorCam);
        camera_to_vector2.Add(mainMirrorCam, new Vector2(10, 75));

        //Set the current camera
        curCam = getEnabledCam();
    }

    // Update is called once per frame
    void Update()
    {
        //Check Touch Controls
        switch (Input.touchCount)
        {
            case 2:
                touchZoom(curCam);
                break;
            //Case 3 is in MainCameraScript for now
            case 3:
                if (mainCam.enabled)
                    StartCoroutine(switchCamerasTouch("RearCamera"));
                else if (rearCam.enabled)
                    StartCoroutine(switchCamerasTouch("MainMirrorCamera"));
                else
                    StartCoroutine(switchCamerasTouch("MainCamera"));
            break;
        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            if (mainCam.enabled) { switchCamerasKeyboard("RearCamera"); }
            else switchCamerasKeyboard("MainCamera");
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (!mainMirrorCam.enabled) switchCamerasKeyboard("MainMirrorCamera");
            else switchCamerasKeyboard("MainCamera");
        }
    }


    /*
     * This function is called when exactly two touches are registered. 
     * It then measures the distance between the previous touch and the new touch. 
     * As well as the distance between these new touches. The cameras field of view 
     * is then changed accordingly.
     * The keep in range function serves to keep it towards a set min and max. In this case
     * these are 7, and 75 respectivley and they were arbitrarily picked by what looked best. 
     */
    public void touchZoom(Camera currentCamera)
    {
        //get touches
        Touch t1 = Input.touches[0];
        Touch t2 = Input.touches[1];

        //calculate prev position
        Vector2 t1Prev = t1.position - t1.deltaPosition;
        Vector2 t2Prev = t2.position - t2.deltaPosition;

        //get magintude (length of the vector)
        float prevMag = (t1Prev - t2Prev).magnitude;
        float currentMag = (t1.position - t2.position).magnitude;

        //Calculate difference
        float diff = currentMag - prevMag;

        Debug.Assert(currentCamera != null);
        //adjust camera FOV
        currentCamera.fieldOfView = keepInRange(currentCamera.fieldOfView - diff * touchZoomSpeed, (int) camera_to_vector2[curCam].x, (int)camera_to_vector2[curCam].y);
    }

    //Helper function to return to other scritps that need raytracing to know which camera to use\
    //Change so it iterates through dictionary and finds enabled
    public Camera getEnabledCam()
    {
        foreach (KeyValuePair<string, Camera> entry in string_to_camera)
        {
            if (string_to_camera[entry.Key].enabled  == true) return string_to_camera[entry.Key];
        }
        Debug.Break(); //No Camera enabled?
        return null;
    }

    /*
     * Reset a given camera to its default location via input values
     */
    public void resetCamera(string camName, Vector3 defaultOffset, Quaternion defaultRotation, float defaultFOV)
    {
        Debug.Assert(string_to_camera[camName] != null);

        string_to_camera[camName].transform.SetLocalPositionAndRotation(defaultOffset, defaultRotation);
        string_to_camera[camName].fieldOfView = defaultFOV;
    }

    public void ResetFOV(string camName, float defaultFOV)
    {
        Debug.Assert(string_to_camera[camName] != null);
        string_to_camera[camName].fieldOfView = defaultFOV;
    }

   
    public void switchCamerasKeyboard(string camName)
    {
        Debug.Assert(string_to_camera[camName] != null);

        foreach(KeyValuePair<string, Camera> entry in string_to_camera)
        {
           if(entry.Key != camName) string_to_camera[entry.Key].enabled = false;
        }

        string_to_camera[camName].enabled = true;
    }

    /*
     * This IEnumerator is needed so we can use a time dealy
     * this is reflected in the yield return statement. Since it is an IEnum (and started as a coroutine)
     * It will not stop all other processes unity is computing. In addition, it is needed so it dose not
     * rapidly change the cameras (on every frame) when 4 touch input is registered, but rather, only after .2 seconds.
     * 
     */
    public IEnumerator switchCamerasTouch(string camName)
    {
        yield return new WaitForSeconds(.2f);
        Debug.Assert(string_to_camera[camName] != null);

        foreach(KeyValuePair<string, Camera> entry in string_to_camera)
        {
           if(entry.Key != camName) string_to_camera[entry.Key].enabled = false;
        }

        (curCam = string_to_camera[camName]).enabled = true;
    }

    //Follow a given target and change camera positon smoothly over a given period of time
    public void follow(Camera cam, Transform target, Vector3 offset, float time)
    {
        Vector3 desiredPosition = target.position + offset; //Automatically uses vector math
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, desiredPosition, ref velocity, time); //Smooths to position over a given time
    }

    public Camera getCamera(string camName)
    {
        Debug.Assert(string_to_camera[camName] != null);
        return string_to_camera[camName];
    }

    //Function to keep elements in a given range
    public float keepInRange(float local, int min, int max)
    {
        if (local > max) return max;
        else if (local < min) return min;
        return local;
    }
}
