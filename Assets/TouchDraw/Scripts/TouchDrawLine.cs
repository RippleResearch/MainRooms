using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchDrawLine : MonoBehaviour
{

    Coroutine drawing;
    public GameObject lineObject;
    //private LineRenderer line;

    void Start(){
        //particles = null;
        //lr = GetComponent<LineRenderer>();
        //SelectLine(particleSystem);
    }

    // Update is called once per frame
    void Update()
    {
        if(!EventSystem.current.IsPointerOverGameObject()){
            //if(particles!=null){
                if(Input.GetMouseButtonDown(0)){
                    StartLine();
                }
                if(Input.GetMouseButtonUp(0)){
                    FinishLine();
                }
            }
        }

            

        // if(Input.touchCount>0){
        // Touch[] touches = Input.touches;
        
        //     foreach (Touch touch in touches){
        //         touch.fingerId;
        //     }

        // }

        // int i = 0;
        // while(i<Input.touchCount){
        //     Touch t = Input.GetTouch(i);
        //     if(t.phase==TouchPhase.Began){
        //         Debug.Log("touch began");
        //         StartLine();
        //     }
        //     else if(t.phase==TouchPhase.Ended){
        //         Debug.Log("touch ended");
        //         FinishLine();
        //     }
        //     else if(t.phase==TouchPhase.Moved){
        //         Debug.Log("touch is moving");
        //     }
        //     i++;


        // }

        //     Touch touch = Input.GetTouch(0);

        //     switch(touch.phase){
        //         case TouchPhase.Began:
        //             rb2D.velocity = new Vector2(0,0);
        //             startPosition = touch.position;

        //             isMouseDown = true;
        //             randomMovementScript.enabled = false;
                    
        //             rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
        //             break;

        //         case TouchPhase.Moved:

        //             Vector3 touchPosition = touch.position; 
        //             touchPosition = cam.ScreenToWorldPoint(touchPosition);

        //             Vector2 direction = new Vector2(touchPosition.x - arrow.transform.position.x, touchPosition.y - arrow.transform.position.y);

        //             arrow.SetActive(true);
        //             transform.up = -direction;
        //             break;

        //         case TouchPhase.Ended:
        //             endPosistion = touch.position;
        //             Vector3 force = startPosition - endPosistion; 

        //             force = new Vector2(force.x,force.y);
        //             rb2D.AddForce(force*launchForce*10);

        //             isMouseDown = false;

        //             arrow.SetActive(false);
        //             rb2D.constraints = RigidbodyConstraints2D.None;
        //             break;

                
        
    

    void StartLine(){
        if(drawing!=null){
            StopCoroutine(drawing);
        }
        drawing = StartCoroutine(DrawLine());
    }

    private void FinishLine(){
        StopCoroutine(drawing);
    }

    IEnumerator DrawLine(){

        // //using a particle system
        // GameObject newGameObject = Instantiate(particles, new Vector3(0,0,0), Quaternion.identity);
        // ParticleSystem line = newGameObject.GetComponent<ParticleSystem>();
        // //line.positionCount = 0;

        // while(true){
        //     Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     mousePosition.z = 0;
        //     //line.positionCount++;
        //     //line.SetPosition(line.positionCount-1,position);
        //     //line.Particle.position = mousePosition;
        //     line.transform.position = mousePosition;
        //     yield return null;
        // }
    
        //  using a lineRenderer
            GameObject newGameObject = Instantiate(lineObject, new Vector3(0,0,0), Quaternion.identity);
            LineRenderer line = newGameObject.GetComponent<LineRenderer>();
            line.positionCount = 0;

            while(true){
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                position.z = 0;
                line.positionCount++;
                line.SetPosition(line.positionCount-1,position);
                yield return null;
            }
    }



    // public void SelectLine(GameObject newParticleSystem){
    //     GameObject newGameObject = Instantiate(newParticleSystem, new Vector3(0,0,0), Quaternion.identity);
    //     ParticleSystem line = newGameObject.GetComponent<ParticleSystem>();
    //     Debug.Log("Line Selected");

    // }
}
