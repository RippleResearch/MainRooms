using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float rotationSpeed = 500.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)){ 
           CanOrbit();
        }
    }

    private void CanOrbit(){
        //to rotate along both axes
        if(Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0){
            float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            float horizontalInput = Input.GetAxis("Mouse X")*rotationSpeed*Time.deltaTime;
            transform.Rotate(Vector3.right,verticalInput);
            transform.Rotate(Vector3.up, horizontalInput, Space.World);
        }

        // if(Input.GetAxis("Mouse X") != 0){ //rotate camera in only the x axis
        // //only works while agent is not moving
        //     float horizontalInput = Input.GetAxis("Mouse X")*rotationSpeed*Time.deltaTime;
        //     transform.Rotate(Vector3.up, horizontalInput, Space.World);
        // }
    }
}
