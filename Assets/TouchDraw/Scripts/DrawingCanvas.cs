using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingCanvas : MonoBehaviour
{

    //public ToggleGroup brushToggleGroup;
    //public GameObject[] brushes;

    public GameObject drawLayer;
    private SpriteRenderer canvasBackground;

    private ObjectSpawn objectSpawnScript;
    private ObjectSpawnTouch objectSpawnTouchScript;
    private ParticleDraw drawScript;
    private ParticleDrawTouch drawTouchScript; 
    private LineDraw lineDrawScript;


    //public GameObject currentBrush; //buttons should be grayed out when you can't use them, like maybe rotation for particle effects 

    public string drawMode = "static";
    public bool canRotate = true; //
    public float size = 3;
    public bool isErasing = false;
    public bool gravityOn = false;


    public string gravityDirection;


    void Start()
    {
        canvasBackground = drawLayer.GetComponent<SpriteRenderer>();

        //references to scripts
        objectSpawnScript = drawLayer.GetComponent<ObjectSpawn>();
        objectSpawnTouchScript = drawLayer.GetComponent<ObjectSpawnTouch>();
        drawScript = drawLayer.GetComponent<ParticleDraw>();
        drawTouchScript = drawLayer.GetComponent<ParticleDrawTouch>();
        lineDrawScript = drawLayer.GetComponent<LineDraw>();

        
        //currentBrush = objectSpawnScript.currentPrefab;

    }

    void Update()
    {
        
    }

    //sets the current brush used for drawing
    public void SetCurrentBrush(GameObject newBrush){
        //currentBrush = newBrush;
        if(newBrush.tag=="Brush"){
            drawScript.particles = newBrush;
            drawTouchScript.particles = newBrush;
            objectSpawnScript.currentPrefab = null;
            objectSpawnTouchScript.currentPrefab = null;
            lineDrawScript.lineObject = null;

        }
        else if(newBrush.tag=="Stamp"){
            objectSpawnScript.currentPrefab = newBrush;
            objectSpawnTouchScript.currentPrefab = newBrush;
            drawScript.particles = null;
            drawTouchScript.particles = null;
            lineDrawScript.lineObject = null;

        }
        else{
            lineDrawScript.lineObject = newBrush;
            objectSpawnScript.currentPrefab = null;
            objectSpawnTouchScript.currentPrefab = null;
            drawScript.particles = null;
            drawTouchScript.particles = null;

        }


    }

    // public void SetCurrentPrefab(GameObject objectPrefab){
    //     objectSpawnScript.currentPrefab = objectPrefab;
    //     objectSpawnTouchScript.currentPrefab = objectPrefab;

    //     drawScript.particles = null;
    //     lineDrawScript.lineObject = null;

    //     lineDrawScript.lineObject = null;

    // }

    // public void SetCurrentLine(GameObject linePrefab){
    //     objectSpawnScript.currentPrefab = null;
    //     objectSpawnTouchScript.currentPrefab = null;

    //     drawScript.particles = null;
    //     drawTouchScript.particles = null;

    //     lineDrawScript.lineObject = linePrefab;

    // }

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

    public int GetOrderInLayer(){
        int order = objectSpawnScript.orderInLayer;
        objectSpawnScript.orderInLayer = order + 1;
        return order;

    }

    public void ClearCanvas(){ //removes all particles and lines, and gameobjects from the canvas
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Brush");
        foreach(GameObject line in lines){
            Destroy(line);
        }
    }

    //switch the drawing mode
    //options are static, collision, and disappearing
    public void ChangeDrawMode(string newDrawMode){
        drawMode = newDrawMode;
    }

    public void IsErasingToggle(Toggle toggle){
        isErasing = toggle.isOn;
    }

    public void SetRotation(Toggle toggle){
        canRotate = !toggle.isOn;
        Debug.Log(canRotate);
    }

    public void SetSize(float newValue){
        size = newValue;
    }

    public void SetGravity(Toggle toggle){
        gravityOn = toggle.isOn;
        gravityDirection = toggle.name;
        switch (toggle.name){ //lerp  between old and new gravity
            case "Up":
                Physics2D.gravity = new Vector3(0f,2f,0);
                //Parti.y = 2f;
                break;
            case "Down":
                Physics2D.gravity  = new Vector3(0f,-2f,0);
                //drawScript.fo.y = -2f;
                break;
            case "Right":
                Physics2D.gravity  = new Vector3(2f,0,0);
                //drawScript.fo.x = 2f;
                break;
            case "Left":
                Physics2D.gravity = new Vector3(-2f,0,0);
                //drawScript.fo.x = -2f;
                break;

        }

        Debug.Log(gravityOn);
        GameObject[] stamps = GameObject.FindGameObjectsWithTag("Stamp");
        foreach(GameObject stamp in stamps){
            Rigidbody2D rb2D =stamp.GetComponent<Rigidbody2D>();
            if(gravityOn){
                rb2D.gravityScale = 1;
            }
            else{
                rb2D.gravityScale = 0;
                rb2D.velocity = Vector2.zero;
            }
        }


    }

    public void ChangeButtonSprite(GameObject toggle){
        Image image = toggle.transform.GetChild(0).GetComponent<Image>();
        Image parentImage = toggle.transform.parent.parent.GetChild(0).GetComponent<Image>();
        //parentImage.sprite = image.sprite;
        parentImage.color = image.color;
    }


}
