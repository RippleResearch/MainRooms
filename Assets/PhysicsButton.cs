using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{

    public Transform buttonTop;
    public Transform buttonLowerLimit;
    public Transform buttonUpperLimit;
    public float threshhold;
    public float force = 10;
    private float upperLowerDiff;
    private bool isPressed;
    private bool prevPresssedState;
    public UnityEvent onPressed;
    public UnityEvent onReleased;
    public UnityEvent onHeld;

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), buttonTop.GetComponent<Collider>());
        if (transform.eulerAngles != Vector3.zero){
            Vector3 savedAngle = transform.eulerAngles;
            transform.eulerAngles = Vector3.zero;
            upperLowerDiff = buttonUpperLimit.position.y - buttonLowerLimit.position.y;
            transform.eulerAngles = savedAngle;
        }
        else{
            upperLowerDiff = buttonUpperLimit.position.y - buttonLowerLimit.position.y;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        buttonTop.transform.localPosition = new Vector3(0, buttonTop.transform.localPosition.y, 0);
        buttonTop.transform.localEulerAngles = new Vector3(0,0,0);
        if(buttonTop.transform.localPosition.y >= 0){ //make sure button does not go above the upper limit
            buttonTop.transform.position = new Vector3(buttonUpperLimit.position.x,buttonUpperLimit.position.y,buttonUpperLimit.position.z);
        }
        else{ //apply spring force to push it back up
            buttonTop.GetComponent<Rigidbody>().AddForce(buttonTop.transform.up *force*Time.fixedDeltaTime);
        }
        if(buttonTop.localPosition.y <= buttonLowerLimit.localPosition.y){ //make sure button does not go below lower limit
            buttonTop.transform.position = new Vector3(buttonLowerLimit.position.x,buttonLowerLimit.position.y,buttonLowerLimit.position.z);
        }

        //compares distance between top of button and lower limit to see if button is pressed based off of the threshhold value
        if (Vector3.Distance(buttonTop.position, buttonLowerLimit.position) < upperLowerDiff*threshhold){
            isPressed = true;
        }
        else{
            isPressed = false;
        }

        if(isPressed && prevPresssedState != isPressed){
            Pressed();
        }
        if(isPressed && prevPresssedState == isPressed){
            Held();
        }
        if(!isPressed && prevPresssedState != isPressed){
            Released();
        }

    }

    void Pressed(){
        prevPresssedState = isPressed;
        onPressed.Invoke();
    }

    void Held(){
        onHeld.Invoke();
    }

    void Released(){
        prevPresssedState = isPressed;
        onReleased.Invoke();
    }
}
