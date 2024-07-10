using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSpawnTouch : MonoBehaviour
{
   public GameObject canvas;
    private DrawingCanvas canvasScript;
    public GameObject currentPrefab;
    //public float spaceBetweenPoints = 1;
    //public bool isMoving = false;
    //public float timeUntilDeactivaton=2;
    //public string drawMode = "static";

    public FlexibleColorPicker fcp;


    //private bool mouseDown = false;
    private Vector2 lastSpawnPoint; //too make sure there isn't too much overlap between particles
    public float spaceBetweenPoints = 1;
    public float timeBetweenPoints = 1;
    public float timer;
    public bool timerOn = false;



    public int orderInLayer = 0;
    //public List<GameObject> objectPool;

    //private bool canDraw = true; //makes sure you can't draw while clicking the ui

    public Sprite[] sprites = new Sprite[4];
    
    public GameObject[] objectsInLastStroke; //undo



    //variable to pass when the object is spawned from the pool
    //timeUntilDeactivation
    //size //original size and deviation
    //amount or space
    //color
    //sprite

    // Start is called before the first frame update
    void Start(){
        canvasScript = canvas.GetComponent<DrawingCanvas>();


    }

    // Update is called once per frame
    void Update()
    {

        if(timerOn){
            timer += Time.deltaTime;
        }
        CheckTouches(); //set flag but process in fixed update??

    }

    // void FixedUpdate(){

    // }


    private void CheckTouches(){
        Debug.Log("Checking Touches");
        if(Input.touchCount>0){
            for(int i=0;i<Input.touchCount;i++){
                //Debug.Log(Input.GetTouch(i).fingerId);
                Touch touch = Input.GetTouch(i);
                Debug.Log(i + " " + touch.fingerId);
                int id = touch.fingerId;

                switch(touch.phase){
                    case TouchPhase.Began:
                        //Debug.Log("Starting New Line "+id);
                        //if(EventSystem.current.IsPointerOverGameObject(id)){
                            //canDraw = false;
                            SpawnObject(touch);
                        //}
                        break;
                    case TouchPhase.Moved:
                        //Debug.Log("Touch is moving "+ id);

                        //if the finger did not start over ui
                        //if(EventSystem.current.IsPointerOverGameObject(id)){
                            SpawnObject(touch); 
                        //}
                        //particleSystems[touch.fingerId].transform.position = Camera.main.ScreenToWorldPoint(touch.position);
                        break;
                    case TouchPhase.Stationary:
                        //Debug.Log("Touch is stationary"+id);
                        break;
                    case TouchPhase.Ended:
                        lastSpawnPoint = Vector2.zero;
                        //canDraw = true;
                        //Destroy(particleSystems[touch.fingerId]);
                        //Debug.Log("The touch has ended"+id); 
                        break;
                    
                }
                
            }

        
        }
    }

    void SpawnObject(Touch touch){
        if(!EventSystem.current.IsPointerOverGameObject(touch.fingerId)){
            if(!canvasScript.isErasing){
                if(currentPrefab!=null){


                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    touchPosition.z = 120; //this allows the draw layer to always be hit first so you can allows draw and erase

                    //make sure they are not overlapping too much
                    if(Vector2.Distance(lastSpawnPoint, touchPosition)>spaceBetweenPoints&&timer>=timeBetweenPoints){
                        
                        if(timerOn){
                            timer = 0;
                        }

                        GameObject newObject = Instantiate(currentPrefab);

                        Rigidbody2D rb2D = newObject.GetComponent<Rigidbody2D>();
                        SpriteRenderer sprite = newObject.GetComponent<SpriteRenderer>();
                        sprite.color = fcp.color;

                        newObject.transform.position = touchPosition;
                        lastSpawnPoint = touchPosition;

                        if(canvasScript.canRotate){
                            newObject.transform.Rotate(0,0,Random.Range(0,360));

                        }

                        // float upperRange; //for size and angle
                        // float lowerRange;
                        float minSizeDifference = 0.05f;
                        float maxSizeDifference = minSizeDifference;
                        float speed = 1;
                        //space between points
                        switch(currentPrefab.name){
                            case "Star":
                                speed = 5;                                
                                break;
                            case "Bubble":
                                break;
                            case "Squiggle":
                                break;
                            case "Smile":
                                minSizeDifference = maxSizeDifference = 0;
                                break;
                            case "Heart":
                                minSizeDifference = 0;
                                maxSizeDifference = .1f;
                                break;
                            case "Pebble":

                                break;
                            case "Fish":
                                break;
                            case "Plant":
                                //no rotation

                                break;
                            case "Tree":
                                break;
                            case "Flower":

                                Color32[] colors = {new Color32(183,58,106,255),new Color32(241,158,190,255),Color.white}; 
                                sprite.color = colors[Random.Range(0,colors.Length)];
                                
                                break;
                            case "Random":
                                minSizeDifference = .1f;
                                maxSizeDifference = -.05f;
                                newObject.transform.Rotate(0,0,Random.Range(0,360));

                                sprite.sprite = sprites[Random.Range(0,sprites.Length)];

                                var gradient = new Gradient();

                                var gradientColors = new GradientColorKey[2];
                                gradientColors[0] = new GradientColorKey(Color.red, 0.0f);
                                gradientColors[1] = new GradientColorKey(Color.blue, 1.0f);

                                var alphas = new GradientAlphaKey[2];
                                alphas[0] = new GradientAlphaKey(1.0f, 1.0f);
                                alphas[1] = new GradientAlphaKey(0.0f, 1.0f);

                                gradient.SetKeys(gradientColors, alphas);

                                sprite.color = gradient.Evaluate(Random.Range(0,1f));;
                                break;
                            default:
                                break;
                        }
        
                        // float randomScale = Random.Range(-minSizeDifference,maxSizeDifference);
                        // newObject.transform.localScale += new Vector3(randomScale,randomScale,0); //should set scale back when deactivating if they are going back into the pool
                        float scale = (canvasScript.size-4)*.05f; //starting size is 3 for the 
                        spaceBetweenPoints =canvasScript.size/4;
                        newObject.transform.localScale += new Vector3(scale,scale,0);

                        newObject.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
                        orderInLayer++; //so the newest object will always be on top //set back to zero when the canvas is cleared
                       
                        Collider2D collider = newObject.GetComponent<Collider2D>();
                        if(canvasScript.drawMode.Equals("disappearing")){
                            //StartCoroutine(WaitAndDeactivate(star)); //destroy instead here?
                            collider.isTrigger = true;
                            rb2D.velocity = Random.insideUnitCircle*speed;
                            Destroy(newObject,10f);
                        }
                        else if(canvasScript.drawMode.Equals("collision")){
                            collider.isTrigger = false;
                            if(canvasScript.gravityOn){
                                //rb2D.bodyType = RigidbodyType2D.Dynamic; 
                                rb2D.gravityScale = 1;
                            }
                            else{
                                //rb2D.bodyType = RigidbodyType2D.Kinematic; 
                            }
                        }
                        else{ 
                            collider.isTrigger = true;
                        }
                    }

                }
            }
            else{ //erasing
                int layer = 7; //layer objects are on
                int layerAsLayerMask = (1<<layer); //only allow erasing on this layer //raycast hits the drawLayer first but we ignore that

                Collider2D collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.position),layerAsLayerMask);

                if(collider!=null){
                    Destroy(collider.gameObject);
                }

            }
        }
    }


    // public void ChangeDrawMode(string newDrawMode){
    //     drawMode = newDrawMode;
    // }

}
