using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class LevelLoader : MonoBehaviour
{
  /*  public GameObject triggerObject;    // object that is not the trigger (island, door, the thing that is being collided with)
    public TMP_Text txt;                // the text box associated with the island
    public Camera cam;
*/
    void Start()
    {
        /*Debug.Assert(triggerObject != null);
        Debug.Assert(txt != null);
        Debug.Assert(cam != null);*/
    }

    
    void Update()
    {
    
    }

    // Should be called in the update method of the child class that inherits this abstract class
    public abstract void CheckHit();

/*    protected abstract void mainMenuClick();*/
}
