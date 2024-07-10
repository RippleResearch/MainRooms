using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWithMouse : MonoBehaviour
{
    public float launchForce;
    //private Vector3 force;
    private Rigidbody rigidBodyComponent;
    private Vector3 startPosition;
    private Vector3 endPosistion;
    private UnityEngine.AI.NavMeshAgent agent;
    private TrailRenderer tr;
    public float time;
    private Camera cam;
    public bool isMouseDown = false;

    //public Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        //rend = GetComponent<Renderer>();        
        rigidBodyComponent = GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        tr = GetComponentInChildren<TrailRenderer>();


    }

    // Update is called once per frame
    void Update()
    {

        //if the snail has not moved in the last 5 seconds and is not currently being clicked by the mouse
        //use enable the navmeshagent to set the destination of the snail to a random position on the play area and with a random speed
        if (rigidBodyComponent.velocity==new Vector3(0,0,0)&&!isMouseDown){
                time+= 1*Time.deltaTime;
            }
        else{
            time = 0;
            if (agent.enabled){
            agent.isStopped = true;
         }

        }


        if (time>=5) {
            if (agent.enabled){
                //agent.SetDestination(transform.position);

                //agent.updatePosition = true;
                //agent.updateRotation = true;
                agent.isStopped = false;

                agent.SetDestination(new Vector3(0,0,0));
                //then give it a new destination

                //once it reaches its destination is needs a new destination
            }
            //Debug.Log("Nav Mesh");
        }




    }

    void OnMouseDown(){
        //stop object from moving
        rigidBodyComponent.velocity = new Vector3(0,0,0);
        startPosition = Input.mousePosition;

        isMouseDown = true;

        //temporarily turn off navmeshagent;
         if (agent.enabled){
        //    // agent.SetDestination(transform.position);
        //     //agent.updatePosition = false;
        //     //agent.updateRotation = false;
             agent.isStopped = true;
         }
    }

    void OnMouseDrag(){
        //increase force as mouse is dragged, based on position of mouse relative to the snail
        //the direction will be opposite of the direction the mouse moves so it can be pulled like a slingshot
        //aim the mouse
        //rend.material.color += Color.white * Time.deltaTime;
        //launchForce += 10 *Time.deltaTime;
        //force = new Vector3(launchForce,0,0);

        //force = new Vector3(mousePosition.x,0,mousePosition.z);
        //add a couple different snail textures and sizes, maybe have a custom snail
        if(Input.GetMouseButtonDown(1)){
            transform.DetachChildren();
            RemoveSnail();
        }

        //show arrow graphic pointing in the direction the mouse is aiming
        //arrow increases in length as the mouse is pulled away from the snail
    }

    void OnMouseUp(){
        endPosistion = Input.mousePosition;
        Vector3 force = startPosition - endPosistion; 
        //control strength and direction
        force = new Vector3(force.x,0,force.y);
        rigidBodyComponent.AddForce(force*launchForce*10);
        //add a maximum force

        isMouseDown = false;

    }

    void RemoveSnail(){
        Destroy(gameObject);
    }

    // void OnCollision(){
    //     agent.SetDestination(new Vector3(-20,1,13));
    // }

    void OnTriggerEnter(Collider other){
        tr.emitting = false; //should not be emitting while the snail is teleporting to the other side
        
        //transform snail to other opening in the border
        if(other.transform.position.z <0){
            transform.position = new Vector3(transform.position.x,1,24);
            //Debug.Log("Lower Trigger");
        }
        else{
            transform.position = new Vector3(transform.position.x,1,-24);
            //Debug.Log("Upper Trigger");
        } //instead of transportimg the entire object would it be possible to transport only the part that went through?
        tr.emitting = true;
    }


}
