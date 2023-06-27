using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSceneLevelLoader : LevelLoader
{
    private GameObject triggerObject;    // object that is not the trigger (island, door, the thing that is being collided with)
    /*public TMP_Text txt;  */              // the text box associated with the island
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CheckHit()
    {
        // main check hit method
        // Implement this by just calling a secondary method that does all the work
        // Do nothing in this one, handle with OnTriggerEnter
    }

    public void OnTriggerEnter(Collider other)
    {
        // objects in the collision needs to be a trigger
        // the other object cannot be a trigger, and needs a rigid body that is kinematic (disabled gravity as well)

        triggerObject = other.gameObject;
        string level = null;
        try
        {
            /*Debug.Log("Name: " + other.gameObject.name);*/
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
            Debug.Log("Exception in WaveSceneLevelLoader Script: " + e.ToString());
        }
        if (other.gameObject.Equals(triggerObject) && level != null)
        {
            SceneManager.LoadScene(level);
        }
    }
}
