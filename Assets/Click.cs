using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Click : MonoBehaviour
{
    Camera m_Camera;
    private NavMeshAgent myAgent;
    private TrailRenderer tr;
   
    void Awake(){
       m_Camera = Camera.main;
    }

    void Start(){

        myAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        tr = GetComponentInChildren<TrailRenderer>();
        tr.emitting = false;
    }

    void Update(){

        //if you want to be able to hold use GetMouseButton
        //otherwise ise GetMouseButtonDown
       if (Input.GetMouseButton(0)){
           Vector3 mousePosition = Input.mousePosition;
           Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 100)) {
                myAgent.SetDestination(hitInfo.point);
                //if the raycast hits an object that doesn't belong on the NavMesh don't move the agent 
                //or only have i register if it is in the same plane as your character until you go down the stairs
                //need to either make camera able to be rotated on its own or have walls turn invisible when you pass through them
               
            }
       }

    //    if (Input.GetMouseButton(1)){
    //     //while the right mouse button is held turn camera


    //    }


        if(Input.GetKeyDown(KeyCode.Space)){
            if(tr.emitting){
                tr.emitting = false;
            }
            else{
                tr.emitting = true;
            }
        }

    }

    void OnCollisionEnter(Collision collision){
                        
        if (collision.gameObject.CompareTag("Button")){ //when the chacter hits a button the trail will be detached and a new trail as the same color of the button with be attached
            transform.DetachChildren(); //also detaches camera
            //set parent of trail to null instead
            TrailRenderer trail = Instantiate(tr);
            if(tr.emitting){
                trail.emitting = true;
            }
            tr = trail;
            tr.transform.parent = transform; //add this trail and child object of the capsule
            tr.GetComponent<TrailRenderer>().material.color = collision.gameObject.GetComponent<Renderer>().material.GetColor("_Color");

        }

    }




















}