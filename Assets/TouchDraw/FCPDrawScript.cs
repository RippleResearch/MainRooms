using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCPDrawScript : MonoBehaviour
{
    public bool getStartingColorFromMaterial = true;
    private FlexibleColorPicker fcp;
    public GameObject currentObject;
    private DrawColorSelect colorSelectScript;
    private bool selected = false;

    private void Start() {
        fcp = gameObject.GetComponent<FlexibleColorPicker>();
        fcp.onColorChange.AddListener(OnChangeColor);
    }

    private void OnEnable(){
        selected = false;
        if(currentObject!=null){
            colorSelectScript = currentObject.GetComponent<DrawColorSelect>();
            if(getStartingColorFromMaterial){

                fcp.color = colorSelectScript.originalColor;
            }
        }
    }

    private void OnChangeColor(Color co) {
        if(!selected){
            colorSelectScript.ColorChanged(co);
        }
        
    }

    public void Cancel(){
        colorSelectScript.ColorChanged(colorSelectScript.originalColor);
        Debug.Log("Cancel");
    }

    public void Done(){
        colorSelectScript.ColorSelected();
        selected = true;
        Debug.Log("Done");

    }
}
