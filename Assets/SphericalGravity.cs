using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalGravity : MonoBehaviour
{

    public GameObject planet;
    public float acceleration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().AddForce((planet.transform.position - transform.position).normalized * acceleration);
        transform.rotation=Quaternion.LookRotation(planet.transform.position-transform.position,transform.up);

        //need to change move script so this works

    }
}
