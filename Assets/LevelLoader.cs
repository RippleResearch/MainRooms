using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameObject mainShip;
    public TMP_Text txt;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(mainShip != null);
        Debug.Assert(txt != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        string level = txt.text;
        Debug.Log(level);
        if (other.gameObject.Equals(mainShip))
        {
            SceneManager.LoadScene(level);
        }
    }
}
