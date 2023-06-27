using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SpawnBoat : ChaseBoat
{
    public GameObject BoatToSpawn;
    
    public float spawnRadius;
    public int multiplier;
    public float shrinkInc = 1;
    public int NumberOfBoats;
    public int maxNumberOfBoats;
    public float timeSinceSpawned;
    public float timeToLive = 30;
    public float timeout = 20;
    
    public float theta;
    public List<Vector3> points;
    
    

    /// <summary>
    /// When Boat is clicked on spawn the BoatToSpawn
    /// Accordingly. Set its location and speed.
    /// </summary>
    public void SpawnBoats() {
        Debug.Break();
        if(Time.time - timeSinceSpawned > timeout) {
            //NumberOfBoats = UnityEngine.Random.Range(1, maxNumberOfBoats);
            theta = 360f / NumberOfBoats;
            Vector3 newOrigin = transform.position;

            //Radius based on number of boats
            spawnRadius = Mathf.Round(NumberOfBoats / 4f); //4 bouts fit on a circle with radius 1
            points = GeneratePoints(transform.position, NumberOfBoats);

            int randomindex = random.Next(points.Count);
            for (int i = 0; i < points.Count; i++) {
                Debug.DrawLine(newOrigin, points[i], UnityEngine.Color.red, 3);
                if(i == randomindex) {
                    Debug.Log("moved real one");
                    transform.position = points[i];
                    SetGivenDestination(5, 5, GetValidCircleLocation(newOrigin, spawnRadius * multiplier, i));                    
                } 
                else {
                    GameObject go = Instantiate(BoatToSpawn, points[i], Quaternion.identity);
                    //go.transform.SetParent(GameObject.FindWithTag("SpawnBoat").transform);
                    go.name = "Special Chase Boat " + i;
                    //Set Ship Values
                    ChaseBoat boatScript = go.GetComponent<ChaseBoat>();
                    SetSpawnBoatValues(boatScript, newOrigin, i);
                }       
            }
            timeSinceSpawned = Time.time;
        }
    }

    public void SetSpawnBoatValues(ChaseBoat boat, Vector3 origin, int index) {
        boat.mainShip = GameObject.Find("Ship").transform;
        boat.SetGivenDestination(5, 5, GetValidCircleLocation(origin, spawnRadius * multiplier, index));
        boat.spawnedBoat = true;
        boat.SetSpeed(100);
        boat.waitMin = boat.waitMax = 2;
        //Destroy after certain amount of time
        StartCoroutine(boat.DestroyAfterTime(timeToLive));
    }

    public List<Vector3> GeneratePoints(Vector3 origin, int numberOfBoats) {
       List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < numberOfBoats; i++) {
            points.Add(GetValidCircleLocation(origin, spawnRadius, i));
        }
        return points;
    }

    public Vector3 GetValidCircleLocation(Vector3 origin, float radius, int index) {
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
