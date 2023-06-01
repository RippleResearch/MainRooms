using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class PatrolBoat : MonoBehaviour
{ 
    //For random wait limits
    protected int waitMin, waitMax;

    //Trims the nav mesh by half so all avaliable locations should be reachable
    [Range(0.0f, 1f)]
    public float trim = .5f;

    //Prefab of the new location they will move towards
    public GameObject LocationPrefab;

    protected NavMeshAgent myAgent;
    protected System.Random random;

    public Vector3 currentDestination;
    
    private Bounds waterBounds;

    
    public void Awake()
    {
        random = new System.Random();
        myAgent = GetComponent<NavMeshAgent>();
        waterBounds = GameObject.Find("WaterSurface").GetComponent<Renderer>().bounds;

        Debug.Assert(myAgent != null); // make sure all boats have nav mesh       
        Debug.Assert(waterBounds != null);
        Debug.Assert(LocationPrefab != null);
    }

    /*
     * General move funtion that sets destiantion
     * and waits a certain amount of time before moving 
     * to it. 
     */
    public IEnumerator move(Vector3 destination, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        myAgent.SetDestination((currentDestination = destination));
    }


    public void beginMove(int minWait, int maxWait)
    {
        Vector3 newLocation = pickRandomPoint();
        initalizeAtPoint(newLocation);
        StartCoroutine(move(newLocation, random.Next(minWait, maxWait)));
    }

    protected void initalizeAtPoint(Vector3 point)
    {
        Instantiate(LocationPrefab, new Vector3(point.x, 0, point.z), LocationPrefab.transform.rotation);
    }

    /*
     * Helper method so we don't destory another boats
     * destination object
     */
    public bool isCorrectLocation(Vector3 location)
    {
        return (location.x == currentDestination.x && location.z == currentDestination.z);
    }

    protected Vector3 pickRandomPoint()
    {
        Vector3 rp;
        do
        {
            rp = new Vector3(
            UnityEngine.Random.Range(waterBounds.min.x * trim, waterBounds.max.x * trim),
            transform.position.y,
            UnityEngine.Random.Range(waterBounds.min.z * trim, waterBounds.max.z * trim)
            );
        } while (rp == currentDestination);

        return rp;
    }

    public void setTurnSpeed(float turnSpeed)
    {
        myAgent.angularSpeed = turnSpeed;
    }

    public void setSpeed(float speed)
    {
        myAgent.speed = speed;
    }

    //Every boat needs to pick there own starting path
    public abstract void Start();
}
