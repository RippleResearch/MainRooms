using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.HighDefinition.ScalableSettingLevelParameter;

public class MainMenuLevelLoader : LevelLoader
{
    [SerializeField] private TMP_Text txt;
    [SerializeField] private GameObject triggerObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckHit();
    }

    public override void CheckHit()
    {
        if(Input.GetKey(KeyCode.Escape)) {
            SceneManager.LoadScene(txt.text);
        }
        // GetMouseButtonDown == (Input.touch.phase == begin)
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Check to make sure raycasting is only in the right scenes
            /*Debug.Log(SceneManager.GetActiveScene().name);*/
            if (Physics.Raycast(ray, out hit))
            {
                ray.origin = Camera.main.transform.position;
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
