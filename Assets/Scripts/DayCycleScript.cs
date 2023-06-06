using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleScript : MonoBehaviour
{
    public float currentTime;
    public float dayLengthMinutes;
    private float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 360 / dayLengthMinutes / 60;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += 1 * Time.deltaTime;
        this.transform.Rotate(new Vector3(1, 0, 0) * rotationSpeed * Time.deltaTime);
        //transform.RotateAround(this.transform.position, Vector3.right, Time.deltaTime * rotationSpeed);
    }
}
