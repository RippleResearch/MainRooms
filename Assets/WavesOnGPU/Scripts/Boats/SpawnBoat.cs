using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpawnBoat : ChaseBoat
{
    public GameObject BoatToSpawn;
    public float spawnRadius = 10;
    public float shrinkInc = 1;
    public int testSlices = 10;
    public List<Vector3> points;

    public override void Start() {
        base.Start();
        SetSpeed(10); SetTurnSpeed(120);

        Debug.Assert(mainShip != null);
    }
    /// <summary>
    /// When Boat is clicked on spawn the BoatToSpawn
    /// Accordingly. Set its location and speed.
    /// </summary>
    public void SpawnBoats() {
        GeneratePoints(transform.position, testSlices);

        for(int i = 0; i < points.Count; i++) {
            Debug.DrawLine(this.transform.position, points[i], Color.red, 3, false);
        }
    }

    public List<Vector3> GeneratePoints(Vector3 origin, int slices) {
        points = new List<Vector3>();
        float theta = 360 / slices;
        Vector3 point;
        
        for (int i = 0; i < slices; i++) {
            float tempRadius = spawnRadius;
            int counter = 0;
            do {
                point = PointOnCircle(this.transform.position, shrinkInc, theta * i);
                tempRadius -= shrinkInc;
                counter++;
                if(counter >= 100) {
                    Debug.Log("Stuck in loop");
                    Debug.Break();
                }
            }
            while (!IsPathValid(point));
            points.Add(point);
        }
        return points;
    }

   public Vector3 PointOnCircle(Vector3 origin, float radius, float theta) {
        float xOffset, zOffset;
        if(theta <= 90) { //Quad 1
            xOffset = (float)Mathf.Cos(theta) * radius;         //+x
            zOffset = (float)Mathf.Sin(theta) * radius;         //+z
        }
        else if(theta <= 180) {//Quad 2
            xOffset = -((float)Mathf.Cos(theta - 90) * radius); //-x
            zOffset = (float)Mathf.Sin(theta - 90) * radius;    //+z
        }
        else if(theta <= 270) { // Quad 3
            xOffset = -((float)Mathf.Cos(theta - 180) * radius); //-x
            zOffset = -((float)Mathf.Sin(theta - 180) * radius); //-z
        }
        else { //Quad 4
            xOffset = (float)Mathf.Cos(theta - 270) * radius;   //+x 
            zOffset = -((float)Mathf.Sin(theta - 270) * radius);//-z
        }
        return new Vector3(origin.x + xOffset, origin.y, origin.z + zOffset);
    }




    
}
