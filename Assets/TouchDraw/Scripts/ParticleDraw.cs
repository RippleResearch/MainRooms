using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ParticleDraw : MonoBehaviour
{

    Coroutine drawing;
    //public GameObject lineObject;
    public GameObject particles; //prefab added in inspector
    private ParticleSystem ps;
    //public Vector3 fo;
    private GameObject newLine;
    public GameObject canvas;
    private DrawingCanvas canvasScript;

    
    //private ParticleSystem line; //the particle system component

    void Start(){
        particles = null;
        ps = GetComponent<ParticleSystem>();
        //canvas = GameObject.Find("Canvas"); //maybe a better way to get order in layer than use find method
        canvasScript = canvas.GetComponent<DrawingCanvas>();
        //SelectLine(particleSystem);
    }

    // Update is called once per frame
    void Update()
    {
        if(particles!=null){
            if(!EventSystem.current.IsPointerOverGameObject()){
                if(Input.GetMouseButtonDown(0)){
                    StartLine();
                }
            }
            if(Input.GetMouseButtonUp(0)){
                FinishLine();
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

                
        
    }

    void StartLine(){
        if(drawing!=null){
            StopCoroutine(drawing);
        }
        drawing = StartCoroutine(DrawLine());
    }

    private void FinishLine(){
        if(drawing!=null){
            StopCoroutine(drawing);
            if(ps!=null){
                ps.Stop();
            }
            Destroy(newLine,20f); //or change depending on how long the particles take to disappear
        }


    }

    IEnumerator DrawLine(){

        //using a particle system
        GameObject newGameObject = Instantiate(particles, new Vector3(0,0,0), Quaternion.identity);
        ParticleSystem line = newGameObject.GetComponent<ParticleSystem>();
        //newGameObject.GetComponent<Renderer>().orderInLayer = order that is in the other script
        //line.positionCount = 0;
        newGameObject.GetComponent<Renderer>().sortingOrder = canvasScript.GetOrderInLayer();

        newLine = newGameObject;
        ps = line;
        if(canvasScript.drawMode.Equals("collision")){
            var col = line.collision;
            col.enabled = true;
            var sub = ps.subEmitters;
            sub.enabled = true;
            Debug.Log("Collision enabled");
        }

        var fo = ps.forceOverLifetime;
        if(canvasScript.gravityOn){
            fo.enabled = true;
            //fo.x = 100;
            Debug.Log("Particle System Gravity On");

            switch (canvasScript.gravityDirection){ 
                case "Up":
                    fo.y = 3f;
                    fo.x = 0;
                    break;
                case "Down":
                    fo.y = -3f;
                    fo.x = 0;
                    break;
                case "Right":
                    fo.x = 3f;
                    fo.y = 0;
                    break;
                case "Left":
                    fo.x = -3f;
                    fo.y = 0;
                    break;

            }
        }
        else{
            fo.enabled = false;
            Debug.Log("Particle System Gravity Off");
        }

        while(true){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            //line.positionCount++;
            //line.SetPosition(line.positionCount-1,position);
            //line.Particle.position = mousePosition;
            line.transform.position = mousePosition;
            yield return null;
        }
        
        
        //  using a lineRenderer
        //     GameObject newGameObject = Instantiate(lineObject, new Vector3(0,0,0), Quaternion.identity);
        //     LineRenderer line = newGameObject.GetComponent<LineRenderer>();
        //     line.positionCount = 0;

        //     while(true){
        //         Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //         position.z = 0;
        //         line.positionCount++;
        //         line.SetPosition(line.positionCount-1,position);
        //         yield return null;
        //     }
    }



    // public void SelectLine(GameObject newParticleSystem){
    //     GameObject newGameObject = Instantiate(newParticleSystem, new Vector3(0,0,0), Quaternion.identity);
    //     ParticleSystem line = newGameObject.GetComponent<ParticleSystem>();
    //     Debug.Log("Line Selected");

    // }
}
