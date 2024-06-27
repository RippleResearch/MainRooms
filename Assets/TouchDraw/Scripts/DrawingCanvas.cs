using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingCanvas : MonoBehaviour
{

    //public ToggleGroup brushToggleGroup;
    //public GameObject[] brushes;
    public GameObject currentBrush;

    public GameObject drawLayer;
    private SpriteRenderer canvasBackground;
    private TouchDraw drawScript;
    private LineDrawer lineScript; 
    private ObjectSpawn objectSpawnScript;
    private ObjectSpawnTouch objectSpawnTouchScript;

    public string drawMode = "disappearing";
    public bool isErasing = false;

    // Start is called before the first frame update
    void Start()
    {
        canvasBackground = drawLayer.GetComponent<SpriteRenderer>();
        drawScript = drawLayer.GetComponent<TouchDraw>();
        lineScript = drawLayer.GetComponent<LineDrawer>();
        objectSpawnScript = drawLayer.GetComponent<ObjectSpawn>();
        objectSpawnTouchScript = drawLayer.GetComponent<ObjectSpawnTouch>();


    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.touchCount>0){
        //     lineScript.DrawNewLine(); //
        // }
        // if(currentBrush!=null){
        //     drawScript.particles = currentBrush;
        //     lineScript.currentBrush = currentBrush;
        // }
    }

    //sets the current brush used for drawing
    public void SetCurrentBrush(GameObject newBrush){
        //currentBrush = newBrush;
        //ClearCanvas();
        objectSpawnScript.objectPrefab = null;
        objectSpawnTouchScript.objectPrefab = null;
        drawScript.particles = newBrush;
        lineScript.currentBrush = newBrush;

    }

    public void SetCurrentPrefab(GameObject objectPrefab){
        //ClearCanvas();
        drawScript.particles = null;
        objectSpawnScript.objectPrefab = objectPrefab;
        objectSpawnTouchScript.objectPrefab = objectPrefab;

    }

    //cycles through color options of the background layer
    public void ChangeCanvasBackground(){
        if (canvasBackground.color==new Color32(60,60,60,255)){
            canvasBackground.color = Color.white;
        }
        else if(canvasBackground.color==Color.white){
            canvasBackground.color = Color.grey;
        }
        else if(canvasBackground.color==Color.grey){
            canvasBackground.color = new Color32(44,42,92,255);
        }
        else if(canvasBackground.color==new Color32(44,42,92,255)){
            canvasBackground.color = Color.black;
        }
        else{
            canvasBackground.color = new Color32(60,60,60,255);
        }
    }

    public void ChangeDrawMode(){
        // if(drawMode=="Line"){
        //     drawMode = "Shape";
        // }
        // else{
        //     drawMode = "Line";
        // }
        ParticleSystem.MainModule psmain = drawScript.particles.GetComponent<ParticleSystem>().main;
        if(drawMode.Equals("disappearing")){ //lines stay put create drawings
            drawMode = "infinite";
            psmain.startLifetime = 10000f;
            Debug.Log("Draw mode: infinite");
        }
        // else if(drawMode.Equals("infinite")){ //touch screen and draw shapes
        //     drawMode = "shape";       
        //     Debug.Log("Draw mode shape");
        //     //psmain.startLifetime = 10000f;
        // }
        else{ //experiment with effects and physics stuff
            drawMode = "disappearing";
            psmain.startLifetime = 5.0f;
            Debug.Log("Draw mode: disappearing");
        }
    }

    
    public void ChangeMode(){
        ClearStars();
        if(drawMode.Equals("disappearing")){
            drawMode = "infinite";
            //stop coroutine of deactivation for particles
        }
        else{
            drawMode = "disappearing";

        }
    }

    public void ClearStars(){
        GameObject[] stars = GameObject.FindGameObjectsWithTag("Stamp");
        objectSpawnScript.orderInLayer = 0;
        objectSpawnTouchScript.orderInLayer = 0;
        foreach(GameObject star in stars){
            if(star.activeInHierarchy){
                //star.SetActive(false);
                Destroy(star);
            }
        }

    }

    public void ClearCanvas(){ //removes all particles and lines from the canvas
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Brush");
        foreach(GameObject line in lines){
            Destroy(line);
        }
    }

    public void IsErasing(){
        if(isErasing){
            isErasing = false;
        }
        else{
            isErasing = true;
        }
    }



}
