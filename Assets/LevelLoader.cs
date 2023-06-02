using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameObject triggerObject;    // object that is not the trigger (island, door, the thing that is being collided with)
    public TMP_Text txt;                // the text box associated with the island
    public Camera cam;

    void Start()
    {
        Debug.Assert(triggerObject != null);
        Debug.Assert(txt != null);
    }

    
    void Update()
    {
        checkHit();
    }

    private void checkHit()
    {
        if(Input.GetMouseButton(0))
        {
            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                ray.origin = cam.transform.position;
                /*Debug.Log("Hit Point: " + hit.point);
                Debug.DrawRay(ray.origin, hit.point, Color.red);*/
                if (hit.collider.isTrigger && hit.collider.gameObject.Equals(triggerObject))
                {
                    string level = txt.text;
                    SceneManager.LoadScene(level);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string level = txt.text;
        Debug.Log(level);
        if (other.gameObject.Equals(triggerObject))
        {
            SceneManager.LoadScene(level);
        }
    }
}
