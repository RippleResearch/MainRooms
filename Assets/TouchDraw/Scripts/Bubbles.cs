using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{

    //public GameObject canvas;
    //private DrawingCanvas canvasScript;
    // Start is called before the first frame update
    void Start()
    {
        //canvasScript = canvas.GetComponent<DrawingCanvas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown(){
        //if(canvasScript.drawMode.Equals("disappearing")){
            this.gameObject.SetActive(false);
        //}
    }

}
