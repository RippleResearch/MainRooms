using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //get all of the positions of the line when it is created
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        Debug.Log("Snail has crossed over the line");
    }
}
