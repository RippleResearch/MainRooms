using JetBrains.Annotations;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SpawnBoat : ChaseBoat
{
    public GameObject BoatToSpawn;
    
    public float spawnRadius = 3;
    public float shrinkInc = 1;
    public int NumberOfBoats = 10;

    public float timeSinceSpawned;
    private float ttl = 5;
    private float timeout = 1;
    public float theta;
    public List<Vector3> points;
    
    

    /// <summary>
    /// When Boat is clicked on spawn the BoatToSpawn
    /// Accordingly. Set its location and speed.
    /// </summary>
    public void SpawnBoats() {
        if(Time.time - timeSinceSpawned > timeout) {
            NumberOfBoats = Random.Range(1,10);
            theta = 360 / NumberOfBoats;

            GeneratePoints(transform.position, NumberOfBoats);
            for (int i = 0; i < points.Count; i++) {
                //Debug.DrawLine(this.transform.position, points[i], UnityEngine.Color.red, 3);
                //Instantiate Boat at point on circal
                GameObject go = Instantiate(BoatToSpawn, points[i], Quaternion.identity);
                go.transform.SetParent(GameObject.FindWithTag("SpawnBoat").transform);
                go.name = "Special Chase Boat " + i;

                //Set Ship Values
                ChaseBoat boatScript = go.GetComponent<ChaseBoat>();
                boatScript.mainShip = GameObject.Find("Ship").transform;;
                boatScript.startLocation = GetValidLocation(transform.position, 20, i);
                
                //Destroy after certain amount of time
                StartCoroutine(boatScript.DestroyAfterTime(ttl));
                timeSinceSpawned = Time.time;
            }
        }
    }

    public List<Vector3> GeneratePoints(Vector3 origin, int slices) {
        points = new List<Vector3>();
        float theta = 360 / slices;
        for (int i = 0; i < slices; i++) {
            points.Add(GetValidLocation(origin, spawnRadius, i));
        }
        return points;
    }

    public Vector3 GetValidLocation(Vector3 origin, float radius, int index) {
        float tempRadius = radius;
        int counter = 0;
        Vector3 point;
        do {
            point = PointOnCircle(origin, tempRadius, theta * index);
            tempRadius -= shrinkInc;
            counter++;

            if (counter >= 100) {
                Debug.Log("Stuck in loop");
                Debug.Break();
            }
        }
        while (!IsPathValid(point));
        return point;
    }

   public Vector3 PointOnCircle(Vector3 origin, float radius, float theta) {
        float xOffset, zOffset;
        theta *= (Mathf.PI/180); // Convert from degress to radians

        if(theta <= 90) { //Quad 1;
            xOffset = (float)Mathf.Cos(theta) * radius;         //+x
            zOffset = (float)Mathf.Sin(theta) * radius;         //+z
        }
        else if(theta <= 180) {//Quad 2
            xOffset = -((float)Mathf.Cos(180 - theta) * radius); //-x
            zOffset = (float)Mathf.Sin(180 - theta) * radius;    //+z
        }
        else if(theta <= 270) { // Quad 3
            xOffset = -((float)Mathf.Cos(270 - theta) * radius); //-x
            zOffset = -((float)Mathf.Sin(270 - theta) * radius); //-z 
        }
        else { //Quad 4
            xOffset = (float)Mathf.Cos(360 - theta) * radius;   //+x 
            zOffset = -((float)Mathf.Sin(360 - theta) * radius);//-z
        }
        return new Vector3(origin.x + xOffset, origin.y, origin.z + zOffset);
    }
}
