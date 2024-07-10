using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawColorSelect : MonoBehaviour
{
    public FlexibleColorPicker fcp;
    private FCPTestScript fcpTest;
    public LineRenderer line;
    public Color32 originalColor;
    public Color32 currentColor;


    
    void Start(){
        line = gameObject.GetComponent<LineRenderer>();
        //GameObject[] flexibleCP = GameObject.FindObjectOfType(FlexibleColorPicker);//GetComponent<FlexibleColorPicker>();
        fcpTest = fcp.GetComponent<FCPTestScript>();

        originalColor = currentColor = line.startColor;

    }

    void Update(){
        
    }

    void OnMouseDown(){
        if(!EventSystem.current.IsPointerOverGameObject()){
            fcpTest.currentObject = this.gameObject;
            fcp.gameObject.SetActive(true);
        }
    }

    public void ColorSelected(){
        originalColor = currentColor;
        Debug.Log("Color Selected");
    }

    public void ColorChanged(Color32 co){
            currentColor = co;
            line.startColor = co;

    }
}
