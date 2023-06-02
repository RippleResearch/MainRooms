using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Made to recognize gestures
/// </summary>
public class TouchController : MonoBehaviour 
{
   
    private float dtt = .3f; //Double tap threshold
    private float lastTapTime;
    public bool listenDoubleTouch()
    {
        if(Input.touchCount == 1)
        {
            Touch t1 = Input.GetTouch(0);
            if(t1.phase == TouchPhase.Began)
            {
                float diff = (Time.time - lastTapTime);
                if (diff <= dtt & diff > .02f)
                {
                    lastTapTime = 0;
                    return true;
                }
                else { lastTapTime = Time.time; }
            }
        }
        return false;
    }

    private Vector2 startPos = new Vector2(0, 0); //Outside method so we store starPos each time
    private Vector2 direction;


    public Vector2 listenTouchPhase(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                startPos = touch.position;
                return startPos;
            case TouchPhase.Moved:
                direction = touch.position - startPos;
                return direction;
            case TouchPhase.Ended:
                return touch.position; //end pos
            default:
                Debug.Log("Nothing to be reported");
                return new Vector2(-1, -1);
        }
    }
}
