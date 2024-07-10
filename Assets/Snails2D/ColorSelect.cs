using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorSelect : MonoBehaviour
{
    public FlexibleColorPicker fcp;
    private FCPTestScript fcpTest;
    public SpriteRenderer sprite;
    public Color32 originalColor;
    public Color32 currentColor;

    

    void Start(){
        sprite = gameObject.GetComponent<SpriteRenderer>();
        fcpTest = fcp.GetComponent<FCPTestScript>();

        originalColor = currentColor = sprite.color;

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
            sprite.color = co;

    }
}
