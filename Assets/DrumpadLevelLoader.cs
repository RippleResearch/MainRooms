using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrumpadLevelLoader : MonoBehaviour
{
   /* public GameObject triggerObject;*/    // object that is not the trigger (island, door, the thing that is being collided with)
    public TMP_Text txt;                // the text box associated with the island
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenuClick()
    {
        SceneManager.LoadScene(txt.text);
    }
}
