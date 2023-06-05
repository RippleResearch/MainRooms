using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImpossibleCubeLevelLoader : LevelLoader
{
    public GameObject triggerObject;    // object that is not the trigger (island, door, the thing that is being collided with)
    public TMP_Text txt;                // the text box associated with the island
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        triggerObject = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CheckHit()
    {
        raycastHit();
    }

    public void raycastHit()
    {
        // GetMouseButtonDown == (Input.touch.phase == begin)
        if (Input.GetMouseButtonDown(0) && SceneManager.GetActiveScene().name.Equals("Impossible Cube"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Check to make sure raycasting is only in the right scenes
            Debug.Log(SceneManager.GetActiveScene().name);
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
}
