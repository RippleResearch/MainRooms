using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalMovement : MonoBehaviour
{

    public Transform gravityTarget;
    //private bool jumpKeyWasPressed;
    //private float horizontalInput;
    //private float verticalInput;
    //private bool isGrounded;
    private Rigidbody rigidBodyComponent;
    //public GameObject planet;
    //public float acceleration;
    public float power = 15000f;
    public float jumpPower = 10f;
    public float torque = 500f;

    public float gravity = 9.8f;
    public bool autoOrient = true;
    public float autoOrientSpeed = 1f;


    //private Trail trail;

    // Start is called before the first frame update
    void Start()
    {

        

        rigidBodyComponent = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    // void Update()
    // {

    //     if(Input.GetKeyDown(KeyCode.Space)){

    //         jumpKeyWasPressed = true;

    //     }
    //     horizontalInput = Input.GetAxis("Horizontal");
    //     veriticalInput = Input.GetAxis("Vertical");

    // }
    
    // private void OnCollision( ){
    //     isGrounded = true;
    // }
    // private void OnCollisionExit(){
    //     isGrounded =false;
    // }
    private void FixedUpdate(){



        // rigidBodyComponent.AddForce((planet.transform.position - transform.position).normalized * acceleration); //or direction*acceleration*mass
        // transform.rotation=Quaternion.LookRotation(planet.transform.position-transform.position,transform.up);

        ProcessInput();
        ProcessGravity();       

        if(Input.GetKeyDown(KeyCode.Space)){

            //rigidBodyComponent.AddRelativeForce(Vector3.up*5, ForceMode.VelocityChange);
            //Vector3 jumpForce = new Vector3();
            rigidBodyComponent.AddRelativeForce(Vector3.up*jumpPower
            , ForceMode.VelocityChange);

        }

        // if (isGrounded){

        // }
        //rigidBodyComponent.velocity = new Vector3(horizontalInput*3,rigidBodyComponent.velocity.y,veriticalInput*3);
    }

    void ProcessGravity(){
        Vector3 diff = transform.position - gravityTarget.position;
        rigidBodyComponent.AddForce(-diff.normalized*gravity*rigidBodyComponent.mass);
        //Debug.DrawRay(transform.position,diff.normalized,Color.red);

        if(autoOrient){
            AutoOrient(-diff);
        }
    }

    void AutoOrient(Vector3 down){
        Quaternion orientationDirection = Quaternion.FromToRotation(-transform.up,down)*transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation,orientationDirection,autoOrientSpeed*Time.deltaTime);
    }

    void ProcessInput(){
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 force = new Vector3(0f,0f,verticalInput*power);
        rigidBodyComponent.AddRelativeForce(force);

        float horizontalInput = Input.GetAxis("Horizontal");
        //Vector3 rforce = new Vector3(0f,horizontalInput*torque,0f);
        Vector3 rforce = new Vector3(horizontalInput*torque,0f,0f);
        rigidBodyComponent.AddRelativeForce(rforce);
    }
}
