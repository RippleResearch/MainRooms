using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayCycleScript : MonoBehaviour
{
    public float currentTime;
    public float dayLengthMinutes;
    private float rotationSpeed;
    //private TMP_Text timeText;
    private string AMPM = "AM";
    private float midday;
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
        currentTime += 1 * Time.deltaTime;
        translateTime = (currentTime / (midday * 2));

        float t = translateTime * 24f;

        float hours = Mathf.Floor(t);

        string displayHours = hours.ToString();
        if(hours == 0)
        {
            displayHours = "12";
        }
        if(hours > 12)
        {
            displayHours = (hours - 12).ToString();
        }
        if(currentTime >= midday)
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
        }

        t *= 60;
        float minutes = Mathf.Floor(t % 60);
        string displayMinutes = minutes.ToString();
        if(minutes < 10)
        {
            displayMinutes = "0"+minutes.ToString();
        }


        string displayTime = displayHours + ":" + displayMinutes + AMPM;
        //timeText.text = displayTime;

        this.transform.Rotate(new Vector3(1, 0, 0) * rotationSpeed * Time.deltaTime);
        //transform.RotateAround(this.transform.position, Vector3.right, Time.deltaTime * rotationSpeed);
    }
}
