using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    void LateUpdate()
    {
        Vector3 camPos = Camera.main.transform.position;
        transform.position = new Vector3(camPos.x, 1, camPos.z);//1 to be above blocks

        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(camPos.z*4f + 20, camPos.x*4f + 20);
    }
}
