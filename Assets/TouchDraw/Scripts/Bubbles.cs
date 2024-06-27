using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{

    public GameObject canvas;
    private DrawingCanvas canvasScript;
    public GameObject drawLayer;
    private ObjectSpawn objectSpawn;

    public bool isErasing = false;


    // Start is called before the first frame update
    void Start()
    {
        //canvasScript = canvas.GetComponent<DrawingCanvas>();
        //objectSpawn = drawLayer.GetComponent<ObjectSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown(){
        //if(canvasScript.drawMode.Equals("disappearing")){
            // this.gameObject.SetActive(false);
        //}
        //if(canvasScript.isErasing){
            Destroy(gameObject);
        //}
        
    }

    // void OnMouseEnter(){
    //     if(isErasing){
    //         Destroy(gameObject);
    //     }
    // }



}
