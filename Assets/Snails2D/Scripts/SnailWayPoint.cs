using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailWayPoint : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWP = 0;

    public float speed = 10.0f;
    public float rotSpeed = 10.0f;
    public float distanceThreshhold = .5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(this.transform.position, waypoints[currentWP].transform.position)<distanceThreshhold){
            currentWP++;
        }

        if(currentWP>=waypoints.Length){
            currentWP = 0;
        }

        //this.transform.LookAt(waypoints[currentWP].transform);

        //Quaternion lookatWp = Quaternion.LookRotation(new Vector2(waypoints[currentWP].transform.position.x,waypoints[currentWP].transform.position.y) - this.GetComponent<Rigidbody2D>().position);
        //transform.right == target.position - transform.position;
        //this.transform.rotation = Quaternion.Slerp(transform.rotation, lookatWp, rotSpeed * Time.deltaTime);

        // float fixedZ = -90;// Or whatever value is needed to set correct facing of your object. Play with this.
        // transform.LookAt( new Vector3(waypoints[currentWP].transform.position.x , waypoints[currentWP].transform.position.y , fixedZ ) );
        // //this.transform.Translate(0,speed*Time.deltaTime,0);
        
        transform.position = Vector2.MoveTowards(transform.position,waypoints[currentWP].transform.position, speed*Time.deltaTime);

    }
}
