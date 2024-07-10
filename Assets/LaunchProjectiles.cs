using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectiles : MonoBehaviour
{

    public GameObject projectile;
    public float launchVelocity = 700f;
    public float rotationSpeed = 10;
    private float y;

    // Start is called before the first frame update
    void Start(){
        y = 0.0f;
    }

    // Update is called once per frame
    void Update(){

        //fire the projectile when the mouse button or control are pressed
        // if (Input.GetButtonDown("Fire1")){ 
        //     fire();
        // }
        
    }

    public void fire(){ //if I make public can access from the button in the inspector
        GameObject ball = Instantiate(projectile, transform.position, transform.rotation);
            ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,launchVelocity,0));
    }

    public void rotate(){

        y += Time.deltaTime * rotationSpeed;
        transform.localRotation = Quaternion.Euler(0,y,0); //rotation works but it set rotation to (0,0,0) when the button is first pressed
        //transform.localRotation  = Quaternion.Euler(transform.localRotation.eulerAngles.x,y,transform.localRotation.eulerAngles.z);
    }
}
