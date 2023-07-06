using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ShipController : MonoBehaviour
{
    [SerializeField] private LayerMask water;
    private NavMeshAgent myAgent;
    private CameraSwitchController cameraSwitchController;
    private WaveController waveController;
    
    //Figure out why you need camera controller on both watersurface and ship to work

    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        cameraSwitchController = GetComponent<CameraSwitchController>();
        waveController = GetComponentInParent<WaveController>();

        Debug.Assert(myAgent != null);
        Debug.Assert(cameraSwitchController != null);
        Debug.Assert(waveController != null);
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        CheckMove();
    }

    //Camera input and move function found in the Main Camera Script (CameraFollow)
    void CheckMove()
    {
        //Check if they are trying to rotate camera using right click
        if(Input.GetMouseButton(1)) { return; }


        //If Left mouse button is being clicked (or held down) 
        //Change to the effect coordinates so we dont have to repeat this action
        if (Input.GetMouseButton(0))
        {
            Ray ray = cameraSwitchController.getEnabledCam().ScreenPointToRay(Input.mousePosition); //Get a ray from mouse position
            if (Physics.Raycast(ray, out var hitInfo, 100)) //If the ray hits a given layer (added in the editor)
            {
                if (hitInfo.transform.gameObject.CompareTag("Water") || hitInfo.transform.gameObject.name.StartsWith("Island")) {
                    myAgent.SetDestination(hitInfo.point); //Then ai will move to that location
                    waveController.effect = new Vector3(hitInfo.textureCoord.x * waveController.resolution.x, hitInfo.textureCoord.y * waveController.resolution.y, waveController.effect.z);
                }
                else if(hitInfo.transform.gameObject.CompareTag("SpawnBoat")) { //if spawn boat clicked spawn boats
                    hitInfo.transform.gameObject.GetComponent<SpawnBoat>().SpawnBoats();
                }
            }        
        }
        //Same thing with touch (only 1 so we can use multi touch for other controls)
        if (Input.touchCount == 1)
        {
            var touchRay = cameraSwitchController.getEnabledCam().ScreenPointToRay(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, cameraSwitchController.getEnabledCam().nearClipPlane));
            if (Physics.Raycast(touchRay, out var hitInfo)) //If the ray hits a given layer (added in the editor)
           {
                if (hitInfo.transform.gameObject.CompareTag("Water") || hitInfo.transform.gameObject.name.StartsWith("Island")) {
                    myAgent.SetDestination(hitInfo.point); //Then ai will move to that location
                    waveController.effect = new Vector3(hitInfo.textureCoord.x * waveController.resolution.x, hitInfo.textureCoord.y * waveController.resolution.y, waveController.effect.z);
                }
                else if (hitInfo.transform.gameObject.CompareTag("SpawnBoat")) { //if spawn boat clicked spawn boats
                    hitInfo.transform.gameObject.GetComponent<SpawnBoat>().SpawnBoats();
                }
            }
        }   
    }
}
