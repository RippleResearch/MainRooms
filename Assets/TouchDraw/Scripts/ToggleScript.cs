using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleScript : MonoBehaviour
{

    public GameObject toggleBrushPrefab;
    public GameObject currentPrefab;
    public GameObject canvas;
    private DrawingCanvas canvasScript;

    void Start()
    {
        canvasScript = canvas.GetComponent<DrawingCanvas>();
        currentPrefab = toggleBrushPrefab;
    }

    //change this toggle's prefab to the prefab of the selected child 
    public void ChangeTogglePrefab(GameObject objectPrefab){
        currentPrefab = objectPrefab;
    }

    //set this toggles current prefab as the current brush in the canvas
    public void SelectTogglePrefab(){
        canvasScript.SetCurrentBrush(currentPrefab);
    }

}
