using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCPTestScript : MonoBehaviour
{
    public bool getStartingColorFromMaterial;
    public FlexibleColorPicker fcp;
    public GameObject currentObject;
    private ColorSelect colorSelectScript;
    private bool selected = false;

    private void Start() {
        fcp.onColorChange.AddListener(OnChangeColor);
    }

    private void OnEnable(){
        selected = false;
        if(currentObject!=null){
            colorSelectScript = currentObject.GetComponent<ColorSelect>();
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
