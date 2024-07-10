using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickingScript : MonoBehaviour
{
    public bool getStartingColorFromMaterial;
    public FlexibleColorPicker fcp;
    public Material material;

    private void Start() {
        if(getStartingColorFromMaterial)
            fcp.color = material.color;

        fcp.onColorChange.AddListener(OnChangeColor);
    }

    private void OnChangeColor(Color co) {
        material.color = co;
    }
}
