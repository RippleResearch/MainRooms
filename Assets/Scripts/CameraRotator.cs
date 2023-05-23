using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        Touch[] touches = Input.touches;
        if(Input.touches.Length > 0)
        {
            Touch touch = touches[0];
            if (touch.position.x > Screen.width / 1.5f && (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
            {
                transform.Rotate(0, -1 * speed * Time.deltaTime, 0);
            }
            else if (touch.position.x < (Screen.width - (Screen.width / 1.5f)) && (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
            {
                transform.Rotate(0, speed * Time.deltaTime, 0);
            }
        }
        
    }


}
