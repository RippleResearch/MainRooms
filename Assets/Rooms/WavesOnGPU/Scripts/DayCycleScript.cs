using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayCycleScript : MonoBehaviour
{
    public float currentTime;
    public float dayLengthMinutes;
    public Material mat1;
    public Material mat2;
    private float rotationSpeed;
    //private TMP_Text timeText;
    /*private string AMPM = "AM";*/
    private float midday;
    private float translateTime;
    
    // Start is called before the first frame update
    void Start()
    {
        mat1.SetFloat("_Blend", 1f);
        // How much time in minutes it will take for the sun to rotate 360 degrees
        rotationSpeed = 360 / dayLengthMinutes / 60;
        midday = dayLengthMinutes * 60 / 2;
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed /6);
        currentTime += 1 * Time.deltaTime;
        translateTime = (currentTime / (midday * 2));

        float t = 6 + translateTime * 24f;

        float hours = Mathf.Floor(t);

        string displayHours = hours.ToString();
        /*if(hours == 0)
        {
            displayHours = "12";
        }*/
        if (hours > 24)
        {
            displayHours = (hours - 24).ToString();
        }

        /*if(currentTime >= midday)
        {
            if (!AMPM.Equals("PM"))
            {
                AMPM = "PM";
            }
            currentTime = 0;
        }
        if (currentTime >= midday * 2)
        {
            if (!AMPM.Equals("AM"))
            {
                AMPM = "AM";
            }
        }*/

        t *= 60;
        float minutes = Mathf.Floor(t % 60);
        string displayMinutes = minutes.ToString();
        if(minutes < 10)
        {
            displayMinutes = "0"+minutes.ToString();
        }


        // 
        float sunset = 1.09f;
        float sunrise1 = 0.92f;
        float sunrise2 = 0.99f;
        float fadeRate = ((midday * 2 * sunrise2) - (midday * 2 * sunrise1)) / 50;

        if ((currentTime%(midday*2) >= midday*sunset) && (currentTime%(midday*2) <= ((midday*2)*sunrise1)))
        {
            RenderSettings.skybox = mat1;
            if(RenderSettings.skybox.GetFloat("_Blend") > 0)
            {
                RenderSettings.skybox.SetFloat("_Blend", RenderSettings.skybox.GetFloat("_Blend") - fadeRate);
            }
            else
            {
                RenderSettings.skybox.SetFloat("_Blend", 0);
            }
        }
        else if (currentTime % (midday * 2) > ((midday * 2) * sunrise1) && currentTime % (midday * 2) <= ((midday * 2) * sunrise2))
        {
            if(RenderSettings.skybox.GetFloat("_Blend") < 1)
            {
                RenderSettings.skybox.SetFloat("_Blend", RenderSettings.skybox.GetFloat("_Blend") + fadeRate);
            }
            else
            {
                RenderSettings.skybox.SetFloat("_Blend", 1);
            }
        }
        else
        {
            mat1.SetFloat("_Blend", 1);
            RenderSettings.skybox = mat2;
        }

        string displayTime = displayHours + ":" + displayMinutes;
        //timeText.text = displayTime;
        /*Debug.Log(displayTime);*/

        this.transform.Rotate(new Vector3(1, 0, 0) * rotationSpeed * Time.deltaTime);
        //transform.RotateAround(this.transform.position, Vector3.right, Time.deltaTime * rotationSpeed);
    }
}
