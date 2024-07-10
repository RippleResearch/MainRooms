using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    private GameObject line;
    private LineRenderer lineRend;

    public float speed = 3;
    float moveSpeed;
    int indexNum;
    public float distanceThreshhold = 0.5f; 
    public int currentPosition;// = 0; //should equals closest point on the line

    // Start is called before the first frame update
    void Start()
    {
        lineRend = line.GetComponent<LineRenderer>();


    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(lineRend.positionCount);
        Vector3[] positions = new Vector3[lineRend.positionCount];
        lineRend.GetPositions(positions);

        if(Vector2.Distance(transform.position, positions[currentPosition])<distanceThreshhold){
            //positions[currentPosition] = lineRend.transform.TransformPoint(positions[currentPosition]);
            currentPosition++;
        }
        transform.position = Vector2.MoveTowards(transform.position,positions[currentPosition], speed*Time.deltaTime);


        // foreach(Vector3 pos in positions){
        //     transform.position = Vector2.MoveTowards(transform.position, pos, speed*Time.deltaTime);
        //     //pos++;

        // }


        // if(Vector2.Distance(this.transform.position, waypoints[currentWP].transform.position)<distanceThreshhold){
        //     currentWP++;
        // }

        // if(currentWP>=waypoints.Length){
        //     currentWP = 0;
        // }
        // transform.position = Vector2.MoveTowards(transform.position,waypoints[currentWP].transform.position, speed*Time.deltaTime);
        // //round lerp value down to int
        // indexNum = Mathf.FloorToInt(moveSpeed);
        // //increase lerp value relative to the distance between points to keep the speed consistent.
        // moveSpeed += speed/Vector3.Distance(positions[indexNum], positions[indexNum+1]);
        // //and lerp
        // transform.position = Vector3.Lerp(positions[indexNum], positions[indexNum+1], moveSpeed-indexNum);
    }

    public void SetLine(GameObject crossedLine, int index){
        line = crossedLine;
        currentPosition = index;
        //also get the position on the line closest to where the snai crossed
    }
}
