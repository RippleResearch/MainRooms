using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMenuController : MonoBehaviour
{
    public void Reposition(int height, int width) {
        transform.position = new Vector3(height / 2f, 1, width / 2f /*- (camPos.z * .08f)*/);//1 to be above blocks
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width * 2f, height * 2f);

        var canvas = GameObject.Find("UI").GetComponent<RectTransform>();
        UpdateCanvas(canvas, rt.sizeDelta);
    }

    public void UpdateCanvas(RectTransform transform, Vector2 pos) {
        transform.sizeDelta = pos;
    }
    
}
