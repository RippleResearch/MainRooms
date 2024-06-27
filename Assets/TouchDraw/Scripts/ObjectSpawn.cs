using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSpawn : MonoBehaviour
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
        // if(Input.GetMouseButton(0)){
        //     Debug.Log("Mouse is Down");
        //     mouseDown = true;
        // }
        // else{
        //     mouseDown = false;
        // }

    }

    void FixedUpdate(){

    }

    void OnMouseDrag(){
        if(!EventSystem.current.IsPointerOverGameObject()){
            if(!canvasScript.isErasing){
                if(objectPrefab!=null){

                
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 120; //120

                    //make sure they are not overlapping too much
                    if(Vector2.Distance(lastSpawnPoint, mousePosition)>spaceBetweenPoints){


                        //WaitAndInstantiate();
                        GameObject star = Instantiate(objectPrefab);


                        //GameObject star = ObjectPool.SharedInstance.GetPooledObject(objectPool);
                        Rigidbody2D rb2D = star.GetComponent<Rigidbody2D>();
                        star.transform.position = mousePosition;
                        lastSpawnPoint = mousePosition;


                        // float upperRange; //for size and angle
                        // float lowerRange;
                        float minSizeDifference = 0.05f;
                        float maxSizeDifference = minSizeDifference;
                        //float speed;
                        //space between points
                        switch(objectPrefab.name){
                            case "Star":
                                star.transform.Rotate(0,0,Random.Range(0,360));
                                Debug.Log("Star");
                                break;
                            case "Bubble":
                                star.transform.Rotate(0,0,Random.Range(0,360));
                                Debug.Log("Bubble");
                                break;
                            case "Squiggle":
                                star.transform.Rotate(0,0,Random.Range(0,360));
                                break;
                            case "Smile":
                                minSizeDifference = maxSizeDifference = 0;
                                break;
                            case "Heart":
                                minSizeDifference = 0;
                                maxSizeDifference = .1f;
                                break;
                            case "Pebble":
                                star.transform.Rotate(0,0,Random.Range(0,360));
                                break;
                            case "Fish":
                                star.transform.Rotate(0,0,Random.Range(0,360));
                                break;
                            case "Plant":
                                //no rotation
                                break;
                            case "Tree":
                                break;
                            default:
                                break;
                        }
        
                        //star.SetActive(true); //if using an object pool

                        float randomScale = Random.Range(-minSizeDifference,maxSizeDifference);
                        star.transform.localScale += new Vector3(randomScale,randomScale,0); //should set scale back when deactivating if they are going back into the pool

                        star.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
                        orderInLayer++; //so the newest object will always be on top //set back to zero when the canvas is cleared

                        if(drawMode.Equals("disappearing")){
                            //Debug.Log("Disappearing Mode");
                            StartCoroutine(WaitAndDeactivate(star)); //destroy instead here?
                            rb2D.velocity = Random.insideUnitCircle;

                        }
                        else{ //draw mode is infinite
                        //Debug.Log("Static mode");
                        }


                        if(canvasScript.isErasing){
                            //star.GetComponent<Bubbles>().isErasing = true;

                        }
                    }

                }
            }
            else{ //erasing
                //ContactFilter2D filter = new ContactFilter2D().NoFilter();
                int layer = 7;
                int layerAsLayerMask = (1<<layer);

                Collider2D collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition),layerAsLayerMask);

                if(collider.name != "DrawLayer"){
                    Debug.Log(collider.name);
                    Debug.Log(collider.name);
                    Destroy(collider.gameObject);

                }

            }
        }
    }

    // IEnumerator WaitAndInstantiate( ){
    //     float timeBetweenInstantiation = 1f;
    //     yield return new WaitForSeconds(timeBetweenInstantiation);
    //     //GameObject star = Instantiate(objectPrefab);
    //     //yield return star;

    // }
    // private void OnPointerDown(PointerEventData eventData){
    //     Debug.Log("pointer down");
    //     if(canvasScript.isErasing){
    //         Debug.Log(eventData.hovered[0].name);
    //     }
    // }

    // void OnMouseDown(){
    //     if(canvasScript.isErasing){
    //         Collider2D collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    //         if(collider.name != "DrawLayer"){
    //             Debug.Log(collider.name);
    //             Destroy(collider.gameObject);

    //         }

    //     }
    // }

    void OnMouseUp(){
        lastSpawnPoint = Vector2.zero;
    }



    //Deactivates the gameObject after 2 seconds
    IEnumerator WaitAndDeactivate(GameObject star){
        float timeUntilDeactivaton = Random.Range(5,10);
        yield return new WaitForSeconds(timeUntilDeactivaton);
        star.SetActive(false);
    }

    public void ChangeDrawMode(string newDrawMode){
        drawMode = newDrawMode;
    }

}
