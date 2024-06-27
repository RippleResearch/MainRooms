using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSpawnTouch : MonoBehaviour
{
   public GameObject canvas;
    private DrawingCanvas canvasScript;
    public GameObject objectPrefab;
    public float spaceBetweenPoints = 1;
    //public bool isMoving = false;
    //public float timeUntilDeactivaton=2;
    public string drawMode = "static";

    //private bool mouseDown = false;
    private Vector2 lastSpawnPoint; //too make sure there isn't too much overlap between particles

    public int orderInLayer = 0;
    //public List<GameObject> objectPool;

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

                if(touch.phase==TouchPhase.Began){
                    //Debug.Log("Starting New Line "+id);
                    SpawnObject(touch);

                }
                else if(touch.phase==TouchPhase.Moved){
                    //Debug.Log("Touch is moving "+ id);
                    SpawnObject(touch);
                    //particleSystems[touch.fingerId].transform.position = Camera.main.ScreenToWorldPoint(touch.position);
                    
                }
                else if(touch.phase==TouchPhase.Stationary){
                    //Debug.Log("Touch is stationary"+id);
                }
                else if(touch.phase==TouchPhase.Ended){
                    //Destroy(particleSystems[touch.fingerId]);
                    //Debug.Log("The touch has ended"+id); 

                }
            }

        
        }
    }

    void SpawnObject(Touch touch){
        if(!EventSystem.current.IsPointerOverGameObject(touch.fingerId)){
            if(!canvasScript.isErasing){
                if(objectPrefab!=null){


                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    touchPosition.z = 120; //this allows the draw layer to always be hit first so you can allows draw and erase

                    //make sure they are not overlapping too much
                    if(Vector2.Distance(lastSpawnPoint, touchPosition)>spaceBetweenPoints){

                        GameObject newObject = Instantiate(objectPrefab);

                        Rigidbody2D rb2D = newObject.GetComponent<Rigidbody2D>();
                        newObject.transform.position = touchPosition;
                        lastSpawnPoint = touchPosition;


                        // float upperRange; //for size and angle
                        // float lowerRange;
                        float minSizeDifference = 0.05f;
                        float maxSizeDifference = minSizeDifference;
                        //float speed;
                        //space between points
                        switch(objectPrefab.name){
                            case "Star":
                                newObject.transform.Rotate(0,0,Random.Range(0,360));
                                Debug.Log("Star");
                                break;
                            case "Bubble":
                                newObject.transform.Rotate(0,0,Random.Range(0,360));
                                Debug.Log("Bubble");
                                break;
                            case "Squiggle":
                                newObject.transform.Rotate(0,0,Random.Range(0,360));
                                break;
                            case "Smile":
                                minSizeDifference = maxSizeDifference = 0;
                                break;
                            case "Heart":
                                minSizeDifference = 0;
                                maxSizeDifference = .1f;
                                break;
                            case "Pebble":
                                newObject.transform.Rotate(0,0,Random.Range(0,360));
                                break;
                            case "Fish":
                                newObject.transform.Rotate(0,0,Random.Range(0,360));
                                break;
                            case "Plant":
                                //no rotation
                                break;
                            case "Tree":
                                break;
                            default:
                                break;
                        }
        
                        float randomScale = Random.Range(-minSizeDifference,maxSizeDifference);
                        newObject.transform.localScale += new Vector3(randomScale,randomScale,0); //should set scale back when deactivating if they are going back into the pool

                        newObject.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
                        orderInLayer++; //so the newest object will always be on top //set back to zero when the canvas is cleared
                       
                        if(drawMode.Equals("disappearing")){
                            //Debug.Log("Disappearing Mode");
                            //StartCoroutine(WaitAndDeactivate(newObject)); //destroy instead here?
                            //Destroy(newObject);
                            rb2D.velocity = Random.insideUnitCircle;
                            Destroy(newObject,5f);

                        }
                        else{ //draw mode is infinite
                        //Debug.Log("Static mode");
                        }
                    }

                }
            }
            else{ //erasing
                int layer = 7; //layer objects are on
                int layerAsLayerMask = (1<<layer); //only allow erasing on this layer //raycast hits the drawLayer first but we ignore that

                Collider2D collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.position),layerAsLayerMask);

                if(collider.name != "DrawLayer"){
                    Debug.Log(collider.name);
                    Debug.Log(collider.name);
                    Destroy(collider.gameObject);

                }

            }
        }
    }


    public void ChangeDrawMode(string newDrawMode){
        drawMode = newDrawMode;
    }

}
