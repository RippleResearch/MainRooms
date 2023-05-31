using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ShipController : MonoBehaviour
{

    [SerializeField] public float turnSpeed;
    [SerializeField] public float speed;
    [SerializeField] private LayerMask whatToClickOn;


    private NavMeshAgent myAgent;
    private CameraSwitchController cameraSwitchController;
    

    private float horizontalInput;
    // Start is called before the first frame update
    void Start()
    { 
        myAgent = GetComponent<NavMeshAgent>();

        cameraSwitchController = GetComponentInParent<CameraSwitchController>();
        Debug.Assert(cameraSwitchController != null);

        //myAgent.updateRotation = false;
        //myAgent.updateUpAxis = false;

        turnSpeed = 120f;
        speed = 10f;
        
       
    }

    // Update is called once per frame
    void Update()
    {
        checkMove();
       
    }

    //Camera input and move function found in the Main Camera Script (CameraFollow)
    void checkMove()
    {
        //Check if they are trying to rotate camera using right click
        if(Input.GetMouseButton(1)) { return; }


        //If Left mouse button is being clicked (or held down) 
        //Change to the effect coordinates so we dont have to repeat this action
        if (Input.GetMouseButton(0))
        {
            Ray ray = cameraSwitchController.getEnabledCam().ScreenPointToRay(Input.mousePosition); //Get a ray from mouse position

            if (Physics.Raycast(ray, out var hitInfo, 100, whatToClickOn)) //If the ray hits a given layer (added in the editor)
            {
                myAgent.SetDestination(hitInfo.point); //Then ai will move to that location
           }        
        }


        //Same thing with touch (only 1 so we can use multi touch for other controls)
        if (Input.touchCount == 1)
        {
            var touchRay = cameraSwitchController.getEnabledCam().ScreenPointToRay(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, cameraSwitchController.getEnabledCam().nearClipPlane));
            if (Physics.Raycast(touchRay, out var hitInfo, 100, whatToClickOn))
            {
                myAgent.SetDestination(hitInfo.point); //Then ai will move to that location
            }
        }
    }   
}
