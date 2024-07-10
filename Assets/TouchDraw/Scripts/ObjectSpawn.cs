using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSpawn : MonoBehaviour
{

    public GameObject canvas;
    private DrawingCanvas canvasScript;
    public GameObject currentPrefab;
    //public float timeUntilDeactivaton=2;

    public FlexibleColorPicker fcp;

    private Vector2 lastSpawnPoint; //too make sure there isn't too much overlap between particles
    public float spaceBetweenPoints = 1;
    public float timeBetweenPoints = 1;
    public float timer;
    public bool timerOn = false;

    public int orderInLayer = 0;
    //public List<GameObject> objectPool;

    private bool canDraw = true; //makes sure you can't draw while clicking the ui

    public Sprite[] sprites = new Sprite[4];
    
    public GameObject[] objectsInLastStroke; //undo


    //variable to pass when the object is spawned from the pool
    //timeUntilDeactivation
    //size //original size and deviation
    //amount or space
    //color
    //sprite

    void Start(){
        canvasScript = canvas.GetComponent<DrawingCanvas>();


    }

    void Update()
    {
        if(timerOn){
            timer += Time.deltaTime;
        }

    }


    void OnMouseDown(){
        if(EventSystem.current.IsPointerOverGameObject()){
            canDraw = false;
        }
    }

    void OnMouseDrag(){
        if(canDraw==true){
            if(!canvasScript.isErasing){
                if(currentPrefab!=null){
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 120;

                    //make sure they are not overlapping too much
                    if(Vector2.Distance(lastSpawnPoint, mousePosition)>spaceBetweenPoints&&timer>=timeBetweenPoints){

                        if(timerOn){
                            timer = 0;
                        }



                        //WaitAndInstantiate(); //instead of space between objects have a set time before another can spawn 
                        GameObject newObject = Instantiate(currentPrefab);
                        //GameObject star = ObjectPool.SharedInstance.GetPooledObject(objectPool);

                        Rigidbody2D rb2D = newObject.GetComponent<Rigidbody2D>();
                        SpriteRenderer sprite = newObject.GetComponent<SpriteRenderer>();
                        sprite.color = fcp.color;

                        newObject.transform.position = mousePosition;
                        lastSpawnPoint = mousePosition;

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

                                //Color32[] randomColors = {new Color32(255,151,0,255),new Color32(75,151,0,255),new Color32(51,149,240,255),new Color32(255,207,21,255)}; 
                                //sprite.color = randomColors[Random.Range(0,randomColors.Length)];

                                var gradient = new Gradient();

                                var gradientColors = new GradientColorKey[2];
                                gradientColors[0] = new GradientColorKey(Color.red, 0.0f);
                                gradientColors[1] = new GradientColorKey(Color.blue, 1.0f);
                                // gradientColors[0] = new GradientColorKey(Color.white, 0.0f);
                                // gradientColors[1] = new GradientColorKey(fcp.color, 1.0f);

                                var alphas = new GradientAlphaKey[2];
                                alphas[0] = new GradientAlphaKey(1.0f, 1.0f);
                                alphas[1] = new GradientAlphaKey(0.0f, 1.0f);

                                gradient.SetKeys(gradientColors, alphas);

                                sprite.color = gradient.Evaluate(Random.Range(0,1f));;
                                break;
                            default:
                                break;
                        }
        
                        //star.SetActive(true); //if using an object pool

                        float scale = (canvasScript.size-4)*.05f; //starting size is 3 for the 
                        spaceBetweenPoints =canvasScript.size/4;
                        newObject.transform.localScale += new Vector3(scale,scale,0);
                        //float randomScale = Random.Range(-minSizeDifference,maxSizeDifference);
                        //newObject.transform.localScale += new Vector3(randomScale,randomScale,0); //should set scale back when deactivating if they are going back into the pool

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
                //ContactFilter2D filter = new ContactFilter2D().NoFilter();
                int layer = 7; //layer objects are on
                int layerAsLayerMask = (1<<layer);//only allow erasing on this layer //raycast hits the drawLayer first but we ignore that

                Collider2D collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition),layerAsLayerMask);

                if(collider!=null){
                    Destroy(collider.gameObject);
                }


            }
        }
    }

    // IEnumerator WaitAndInstantiate( ){
    //     float timeBetweenInstantiation = 1f;
    //     yield return new WaitForSeconds(timeBetweenInstantiation);
    //     //GameObject star = Instantiate(currentPrefab);
    //     //yield return star;

    void OnMouseUp(){
        lastSpawnPoint = Vector2.zero;
        canDraw = true;
    }

    // public void ApplyGravity(GameObject newObject, Vector2 direction){
        
    // }

    //Deactivates the gameObject after 2 seconds
    IEnumerator WaitAndDeactivate(GameObject star){
        float timeUntilDeactivaton = Random.Range(5,10);
        yield return new WaitForSeconds(timeUntilDeactivaton);
        star.SetActive(false);
    }



}
