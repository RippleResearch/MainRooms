using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private bool jumpKeyWasPressed;
    private float horizontalInput;
    private float veriticalInput;
    private bool isGrounded;
    private Rigidbody rigidBodyComponent;
    

    //private Trail trail;

    // Start is called before the first frame update
    void Start()
    {
        rigidBodyComponent = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space)){

            jumpKeyWasPressed = true;

        }
        horizontalInput = Input.GetAxis("Horizontal");
        veriticalInput = Input.GetAxis("Vertical");

    }
    
    private void OnCollision( ){
        isGrounded = true;
    }
    private void OnCollisionExit(){
        isGrounded =false;
    }
    private void FixedUpdate(){

        if(jumpKeyWasPressed == true){

            rigidBodyComponent.AddForce(Vector3.up*5, ForceMode.VelocityChange);
            jumpKeyWasPressed = false;

        }

        if (isGrounded){
            //trail;
        }
        rigidBodyComponent.velocity = new Vector3(horizontalInput*3,rigidBodyComponent.velocity.y,veriticalInput*3);
    }
}
