using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SnailScript : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private SnailWayPoint snailWPScript;
    private RandomMovement randomMovementScript;
    private FollowPath followPathScript;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private TrailRenderer tr;
    private GameObject arrow;
    private GameObject trail;
    public float time;
    private Camera cam;

    public float launchForce = 3.0f;
    //public float speed = 5.0f;
    public float timeToWait = 5.0f;

    public bool isMouseDown = false;
    //public bool isMoving = false;
    public bool randomMovementOn = false;
    public bool canTeleport = true;

    public GameObject impact;
    //public bool stopTimer = false;
    //public bool isTeleporting = false;
    //public bool teleported = false;

    public LayerMask touchInputMask;

    //for teleporting
    private GameObject previousSnail; //if this snail was a copy
    private GameObject newSnail; //the snail that is a copy of this one

    //for line trail
    private GameObject newGameObject;
    private LineRenderer line;
    private EdgeCollider2D edgeCollider;
    List<Vector2> points = new List<Vector2>();

    public bool pathmode = false;




    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        rb2D = GetComponent<Rigidbody2D>();

        //scripts
        snailWPScript = GetComponent<SnailWayPoint>();
        randomMovementScript = GetComponent<RandomMovement>();
        followPathScript = GetComponent<FollowPath>();

        trail = transform.GetChild(0).gameObject;
        tr = trail.GetComponent<TrailRenderer>();
        arrow = transform.GetChild(1).gameObject;

        //newGameObject = transform.GetChild(0).gameObject;
        // line = newGameObject.GetComponent<LineRenderer>();
        // line.positionCount = 0;
        // edgeCollider = newGameObject.GetComponent<EdgeCollider2D>();
        

    }

    // Update is called once per frame
    void Update()
    {

        //decide movement
        //if mouse is down
        //else if following a line
        //else random movement

        // if(rb2D.velocity.x<0.01&&rb2D.velocity.y<0.01){
        //     rb2D.velocity = Vector2.zero;
        // } //problem is stops moving when it hits the wall

        //if the snail has not moved in the last 5 seconds and is not currently being clicked by the mouse
        //use enable the navmeshagent to set the destination of the snail to a random position on the play area and with a random speed
        if (rb2D.velocity==new Vector2(0,0)&&!isMouseDown&&!randomMovementOn){//&&!stopTimer){
                time+= 1*Time.deltaTime;
            }
        else{
            time = 0;
        }

        if (time>=5) {
            randomMovementScript.enabled = true;
            //snailWPScript.enabled = true;

            //time = 0;
            //stopTimer = true;
        }

        // if(Input.touchCount>0){
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

        //     }
            
        // }

     
        // //creates a line trail
        // Vector3 position = transform.position;
        // position.z = 0;
        // line.positionCount++;
        // points.Add(position);
        // line.SetPosition(line.positionCount-1,position);
        // //edge renderer being changed in local cooridinates
        // edgeCollider.points = points.ToArray();

    }

    void OnMouseDown(){
        //stop object from moving
        if(!EventSystem.current.IsPointerOverGameObject()&&pathmode==false) {
            rb2D.velocity = new Vector2(0,0);
            startPosition = Input.mousePosition;//or maybe center of snail would be better?
            //probably shouldn't be able to click on the snail through the ui

            isMouseDown = true;
            //snailWPScript.enabled = false;
            randomMovementScript.enabled = false;
            
            rb2D.constraints = RigidbodyConstraints2D.FreezeAll;}
        //FreezeRotation;
        
    }


    void OnMouseDrag(){
        //increase force as mouse is dragged, based on position of mouse relative to the snail
        //the direction will be opposite of the direction the mouse moves so it can be pulled like a slingshot
        //aim the mouse
        if(!EventSystem.current.IsPointerOverGameObject()&&pathmode==false){

            if(Input.GetMouseButtonDown(1)){
                //transform.DetachChildren();
        
                RemoveSnail();
            }

            Vector3 mousePosition = Input.mousePosition; 
            mousePosition = cam.ScreenToWorldPoint(mousePosition);

            Vector2 direction = new Vector2(mousePosition.x - arrow.transform.position.x, mousePosition.y - arrow.transform.position.y);

            //when mouse leaves the snail's body the arrow should appear
            arrow.SetActive(true);

            // LineRenderer line = arrow.transform.GetChild(0).GetComponent<LineRenderer>();
            // Transform arrowPointer = arrow.transform.GetChild(1);
            // Vector2 linePosition = line.GetPosition(1);
            // float distance = Vector2.Distance(rb2D.position, mousePosition);
            // line.SetPosition(1,new Vector2(0,distance));
            // arrowPointer.localPosition = new Vector2(0,distance);
            


            // var arrowSize = transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
            // arrowSize.transform.localScale += arrowSize.transform.localScale;//+= arrowSize.transform.localScale*1/1000000*Vector3.Distance(mousePosition, Input.mousePosition);
            // arrowSize =  arrowSize*2;

            //arrow.transform.up = -direction;
            transform.up = -direction; //rotates snail in the direction it will be launched
            //change length of arrow according to how far the mouse is pulled
        }

    }

    void OnMouseUp(){
        
        if(!EventSystem.current.IsPointerOverGameObject()&&pathmode==false){

            endPosition = Input.mousePosition;
            Vector3 force = startPosition - endPosition; 
            //control strength and direction
            force = new Vector2(force.x,force.y);
            rb2D.AddForce(force*launchForce*10);
            //add a maximum force?

            isMouseDown = false;
            //stopTimer = false;

            arrow.SetActive(false);
            rb2D.constraints = RigidbodyConstraints2D.None;
        }


    }

    public void RemoveSnail(){
        trail.transform.parent = null;
        Destroy(gameObject);
        Destroy(trail, 10f);
    }

    void OnTriggerEnter2D(Collider2D other){

        if(other.gameObject.tag=="Portal"&&canTeleport){

            //newSnail


            //randomMovementScript.enabled = false;
            RandomMovement(false);
            //transform snail to other opening in the border
            //they need the same velocity until one exits the portal completely and the one outside the game needs to be destroyed

            //otherSnail = newSnail

            if(other.transform.position.y <0){ //lower triggers
                //rb2D.position = new Vector2(rb2D.position.x,5);
                GameObject newSnail = Instantiate(this.gameObject,new Vector2(rb2D.position.x,6.77f), Quaternion.identity);
                //var newSnailScript = newSnail.GetComponent<SnailScript>();
                //newSnailScript.canTeleport = false;
                newSnail.GetComponent<Rigidbody2D>().velocity = rb2D.velocity;
                //Destroy(gameObject,2f);
                
            }
            else{ //upper triggers
                GameObject newSnail = Instantiate(this.gameObject,new Vector2(rb2D.position.x,-6.77f), Quaternion.identity); //transform.rotation? //instead of the number it should be the radius down from the edge of the screen
                newSnail.GetComponent<SnailScript>().canTeleport = false;

                //newSnail.transform.parent = gameObject.transform;
                newSnail.GetComponent<Rigidbody2D>().velocity = rb2D.velocity;
                //Destroy(gameObject,2f); 



                //rb2D.position = new Vector2(rb2D.position.x,-5);
                //Debug.Log("Upper Trigger");
            }
            //newSnailScript.previousSnail = this.gameObject; 


        }
        if(other.gameObject.tag=="Path"){
            Debug.Log("Snail has crossed over a line");

            followPathScript.enabled = true;
            followPathScript.SetLine(other.gameObject, 20);
            


            //travel to the next on in line until you reach the end of the trail then resume random movement 
        }

  
    }

    void OnTriggerStay2D(){
        // if(previousSnail.velocity != rb2D.velocity){
        //     rb2D.velocity = previousSnail.GetComponent<Rigidbody2D>().velocity;
        // }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag=="Portal"){
            canTeleport = true;

            //if it goes to the outside of the trigger destroy the game object
            if(Mathf.Abs(rb2D.position.y)>7){
                //unchild the other snail
                trail.transform.parent = null;
                Destroy(gameObject,2f);
                Destroy(trail, 10f);
            }

        }
        // if(other.gameObject.tag=="Path") {
        //     followPathScript.enabled = false;
        // }
    }

    // void OnCollisionEnter2D(Collision2D collision){
        
    //     GameObject NewImpact = Instantiate(impact, collision.GetContact(0).point + collision.GetContact(0).normal * 0.001F, collision.transform.rotation);

    //     Destroy(NewImpact, 2f);
        
    // }
    // void OnCollisionEnter2D(){
    //     if(!isMouseDown){
    //         randomMovementScript.enabled = true;
    //     }
    // }

    public void RandomMovement(bool value){
        randomMovementOn = value; //stops random movement and timer until set to true
        randomMovementScript.enabled = value;
    }

}
