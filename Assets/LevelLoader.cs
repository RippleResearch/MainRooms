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
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("1");
                if (hit.collider.isTrigger)
                {
                    Debug.Log("2");
                    //Do the thing
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
