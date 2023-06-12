using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    private float rotationSpeed;
    private float midday;
    public float currentTime;
    public float dayLengthMinutes;
    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 360 / dayLengthMinutes / 60;
        midday = dayLengthMinutes * 60 / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rotate()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed / 6);
    }

    public float GetMidday()
    {
        return midday;
    }

    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }

    public void SetRotationSpeed(float var)
    {
        rotationSpeed = var;
    }

    public void SetMidday(float var)
    {
        midday = var;
    }

    public virtual void Cycle()
    {

    }

}
