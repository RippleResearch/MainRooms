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
        /*Debug.Assert(triggerObject != null);
        Debug.Assert(txt != null);
        Debug.Assert(cam != null);*/
    }

    
    void Update()
    {
        checkHit();
    }

    private void checkHit()
    {
        // GetMouseButtonDown == (Input.touch.phase == begin)
        if(Input.GetMouseButtonDown(0) && SceneManager.GetActiveScene().name.Equals("Impossible Cube"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Check to make sure raycasting is only in the right scenes
/*            Debug.Log(SceneManager.GetActiveScene().name);*/
            if (Physics.Raycast(ray, out hit))
            {
                ray.origin = cam.transform.position;
                // Check if the thing the ray is hitting is a trigger, and equals 
                if (hit.collider.isTrigger && hit.collider.gameObject.Equals(triggerObject))
                {
                    // Child object of the trigger object is a text TMP, string value of that is the level name
                    string level = txt.text;
                    SceneManager.LoadScene(level);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // objects in the collision needs to be a trigger
        // the other object cannot be a trigger, and needs a rigid body that is kinematic (disabled gravity as well)

        triggerObject = other.gameObject;
        string level = null;
        try
        {
            Debug.Log("Name: " + other.gameObject.name);
            if (other.gameObject.name.ToLower().Contains("island"))
            {
                /*Debug.Log("Name: " + other.gameObject.name);
                Debug.Log("1");*/
                level = other.GetComponentInChildren<TMP_Text>().text;
                /*Debug.Log("Level: " + level);*/
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        if (other.gameObject.Equals(triggerObject) && level != null)
        {
            SceneManager.LoadScene(level);
        }
    }
}
