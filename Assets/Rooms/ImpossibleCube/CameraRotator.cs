using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
        if (Input.GetMouseButton(0))
        { 
            Vector3 mouse = Input.mousePosition;
            if (mouse.x > Screen.width / 1.5f /*&& (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)*/)
            {
                transform.Rotate(0, -1 * speed * Time.deltaTime, 0);
            }
            else if (mouse.x < (Screen.width - (Screen.width / 1.5f)) /*&& (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)*/)
            {
                transform.Rotate(0, speed * Time.deltaTime, 0);
            }
        }

    }


}
