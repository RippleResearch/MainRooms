using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotate : MonoBehaviour
{
    public float currentTime;
    public float dayLengthMinutes;
    private float rotationSpeed;
    private float midday;
    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 360 / dayLengthMinutes / 60;
        midday = dayLengthMinutes * 60 / 2;
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed / 6);
    }
}
