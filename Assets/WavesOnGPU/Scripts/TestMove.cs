using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
/*using Unity.VisualScripting;*/
using UnityEngine;

public class TestMove : MonoBehaviour
{
    private WaveController waveController;
    private CameraSwitchController cameraSwitchController;
  

    // Start is called before the first frame update
    void Start()
    {
        waveController = GetComponent<WaveController>();
        cameraSwitchController = GetComponentInParent<CameraSwitchController>();       
        Debug.Assert(cameraSwitchController != null);
       
    }

    // Update is called once per frame
    void Update()
    {

        Camera cam = cameraSwitchController.getEnabledCam();
        var mousePos = Input.mousePosition;
        
        //Check if player is trying to change rotation
        if (Input.GetMouseButton(1)) { return; }

        //Check if left mouse but clicked
        if (Input.GetMouseButton(0))
        {
            var ray = cam.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
            //Check if it hit the game object
            if (Physics.Raycast(ray, out var hitInfo) && hitInfo.collider.gameObject == gameObject)
            {
                //Translate effects cordinates to the texture cordinates (adjusted by resolution) on the material 
                waveController.effect = new Vector3(hitInfo.textureCoord.x * waveController.resolution.x, hitInfo.textureCoord.y * waveController.resolution.y, waveController.effect.z);
            }

        }

        //Same thing with touch (only 1 so we can use multi touch for other controls)
        if(Input.touchCount == 1)
        {
            var touchRay = cam.ScreenPointToRay(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, cam.nearClipPlane));
            if (Physics.Raycast(touchRay, out var hitInfo) && hitInfo.collider.gameObject == gameObject)
            {
                waveController.effect = new Vector3(hitInfo.textureCoord.x * waveController.resolution.x, hitInfo.textureCoord.y * waveController.resolution.y, waveController.effect.z);
            }
        }

    }
}

