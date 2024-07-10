using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineDraw : MonoBehaviour
{

    Coroutine drawing;
    public GameObject lineObject;
    public GameObject canvas;
    private DrawingCanvas canvasScript;

    public FlexibleColorPicker fcp;
    //private LineRenderer line;

    void Start(){
        canvasScript = canvas.GetComponent<DrawingCanvas>();

    }

    void Update(){

        //if lineObject is line
        //else if it is particle system
        //else it is null or something else
        if(lineObject!=null){ 

            if(Input.GetMouseButtonDown(0)){
                if(!EventSystem.current.IsPointerOverGameObject()){

                    StartLine();
                }
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
        if(drawing!=null){
            StopCoroutine(drawing);
        }
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

            EdgeCollider2D edgeCollider = newGameObject.GetComponent<EdgeCollider2D>();
            List<Vector2> points = new List<Vector2>();
            if(canvasScript.drawMode.Equals("collision")){
                edgeCollider.isTrigger = false;
            }
            edgeCollider.edgeRadius = canvasScript.size*.05f;

            newGameObject.GetComponent<Renderer>().sortingOrder = canvasScript.GetOrderInLayer();

            line.endColor = line.startColor = fcp.color;
            line.startWidth = line.endWidth = canvasScript.size*.1f;
            //Debug.Log(canvasScript.size);

            if(canvasScript)

            while(true){
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                position.z = 0;
                line.positionCount++;

                points.Add(position);


                line.SetPosition(line.positionCount-1,position);
                
                
                edgeCollider.points = points.ToArray();


                yield return null;
            }
    }






    void SetSingleColor(LineRenderer lineRendererToChange, Color newColor){
        lineRendererToChange.startColor = newColor;
        lineRendererToChange.endColor = newColor;
    }

    void SetSingleColor2(LineRenderer lineRendererToChange,Color newColor){
        Gradient tempGradient = new Gradient();

        GradientColorKey[] tempColorKeys = new GradientColorKey[2];
        tempColorKeys[0] = new GradientColorKey(newColor,0);
        tempColorKeys[1] = new GradientColorKey(newColor,1);
        tempGradient.colorKeys = tempColorKeys;
        lineRendererToChange.colorGradient = tempGradient;
    }

    Color RandomColor(){
        return Random.ColorHSV();
    }

    IEnumerator RandomSingleColorMorphing(LineRenderer lineRendererToChange, float timeToMorph){
        Debug.Log("Color Changing");
        float time = 0;
        Color initialColor = lineRendererToChange.colorGradient.colorKeys[0].color;
        SetSingleColor2(lineRendererToChange,initialColor);
        while(true){
            initialColor = lineRendererToChange.colorGradient.colorKeys[0].color;
            Color targetColor = Random.ColorHSV();
            time = 0;
            while(time<timeToMorph){
                time+=Time.deltaTime;
                float progress = time/timeToMorph;
                Color currentColor = Color.Lerp(initialColor,targetColor,progress);
                SetSingleColor(lineRendererToChange,currentColor);
                yield return null;
            }
            yield return null;
            
        }
    }

}
