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
    [SerializeField] private string AMPM = "AM";
    [SerializeField] private float midday;
    private float translateTime;
    
    // Start is called before the first frame update
    void Start()
    {
        // How much time in minutes it will take for the sun to rotate 360 degrees
        rotationSpeed = 360 / dayLengthMinutes / 60;
        midday = dayLengthMinutes * 60 / 2;
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed /8);
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

        if (currentTime%(midday*2) >= midday /*&& currentTime <= midday * 1.5f*/)
        {
            RenderSettings.skybox = mat1;
        }
        else
        {
            RenderSettings.skybox = mat2;
        }

        string displayTime = displayHours + ":" + displayMinutes;
        //timeText.text = displayTime;
        Debug.Log(displayTime);

        this.transform.Rotate(new Vector3(1, 0, 0) * rotationSpeed * Time.deltaTime);
        //transform.RotateAround(this.transform.position, Vector3.right, Time.deltaTime * rotationSpeed);
    }
}
