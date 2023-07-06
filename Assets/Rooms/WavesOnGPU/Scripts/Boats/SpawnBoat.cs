using System.Collections.Generic;
using UnityEngine;
public class SpawnBoat : ChaseBoat
{
    public GameObject BoatToSpawn;
    
    public int multiplier;
    public float shrinkInc = 1;
    public int NumberOfBoats;
    public int maxNumberOfBoats;
    public float timeSinceSpawned;
    public float timeToLive = 30;
    public float timeout = 20;
   
    /// <summary>
    /// When Boat is clicked on spawn the BoatToSpawn
    /// Accordingly. Set its location and speed.
    /// </summary>
    public void SpawnBoats() {
        //Debug.Break();
        if(Time.time - timeSinceSpawned > timeout) {
            NumberOfBoats = UnityEngine.Random.Range(1, maxNumberOfBoats);
             Vector3 newOrigin = transform.position;
            //Radius based on number of boats

            int randomindex = random.Next(NumberOfBoats);
            int currRadius = 1;
            int counter = 0;
            while(NumberOfBoats > 0) {
                
                List<Vector3> points = new List<Vector3>();
                float theta;
                if (NumberOfBoats >= currRadius * 4) {
                    theta = 360f / (currRadius * 4);
                    points = GeneratePoints(newOrigin, currRadius, theta, currRadius * 4);
                }
                else {
                    theta = 360f / NumberOfBoats;
                    points = GeneratePoints(newOrigin, currRadius, theta, currRadius * 4);
                }
               
                for(int i = 0; i < points.Count; i++) {
                    if(randomindex == NumberOfBoats) {
                        transform.position = points[i];
                        SetGivenDestination(5, 5, GetValidCircleLocation(
                            newOrigin, currRadius * multiplier, theta * i));
                    }
                    else {
                        GameObject go = Instantiate(BoatToSpawn, points[i], Quaternion.identity);
                        go.name = "Special Chase Boat " + NumberOfBoats;
                        //Set Ship Values
                        ChaseBoat boatScript = go.GetComponent<ChaseBoat>();
                        SetSpawnBoatValues(boatScript, newOrigin, currRadius, theta * i);
                    }
                    //Dec number of boats
                    NumberOfBoats--;
                    if (NumberOfBoats == 0) break;
                }
                currRadius += 2;
                if (++counter > 100) Debug.Break();
            }    
            timeSinceSpawned = Time.time;
        }
    }

    public void SetSpawnBoatValues(ChaseBoat boat, Vector3 origin, float radius, float theta) {
        boat.mainShip = GameObject.Find("Ship").transform;
        boat.SetGivenDestination(5, 5, GetValidCircleLocation(origin, radius * multiplier, theta));
        boat.spawnedBoat = true;
        boat.SetSpeed(10);
        boat.waitMin = boat.waitMax = 2;
        //Destroy after certain amount of time
        StartCoroutine(boat.DestroyAfterTime(timeToLive));
    }

    public List<Vector3> GeneratePoints(Vector3 origin, float radius, float theata, int numberOfBoats) {
       List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < numberOfBoats; i++) {
            points.Add(GetValidCircleLocation(origin, radius, theata * i));
        }
        return points;
    }

    public Vector3 GetValidCircleLocation(Vector3 origin, float radius, float theta) {
        float tempRadius = radius;
        int counter = 0;
        Vector3 point;
        do {
            point = PointOnCircle(origin, tempRadius, theta);
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
