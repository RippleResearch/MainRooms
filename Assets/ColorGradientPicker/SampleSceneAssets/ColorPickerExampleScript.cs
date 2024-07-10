using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColorPickerExampleScript : MonoBehaviour
{
    private Renderer rend;
    //void Start()
    //{
    //    r = GetComponent<Renderer>();
    //    r.sharedMaterial = r.material;
    //}
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.sharedMaterial = rend.material;
    }


   
    public void ChooseColorButtonClick()
    {
        ColorPicker.Create(rend.sharedMaterial.color, "Choose color!", SetColor, ColorFinished, true);
       
    }
   
    private void SetColor(Color currentColor)
    {
        rend.sharedMaterial.color = currentColor;
    }


    private void ColorFinished(Color finishedColor)
    {
        Debug.Log("You chose the color " + ColorUtility.ToHtmlStringRGBA(finishedColor));
    }
   
    void OnMouseDown()
    {
       if (Input.GetMouseButtonDown(0))
        {
            // Open the color picker dialog when the object is clicked
            ChooseColorButtonClick();
        }
    }


    // Method to change the color of the GameObject
    private void ChangeColor()
    {
        // Change the color of the GameObject to a random color
        rend.sharedMaterial.color = UnityEngine.Random.ColorHSV();
    }
}
