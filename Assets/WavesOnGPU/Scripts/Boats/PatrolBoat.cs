using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class PatrolBoat : MonoBehaviour
{

    public float speed; //Change so these effect navmesh
    public float turnSpeed;

    [Range(0.0f, 1f)]
    public float trim = .5f;

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


        //Start movement and update currentDestination
        beginMove(0, 10);
    }

    /*
     * General move funtion that sets destiantion
     * and waits a certain amount of time before moving 
     * to it. 
     */
    public IEnumerator move(Vector3 destination, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Destination Set");
        myAgent.SetDestination((currentDestination = destination));
    }


    public void beginMove(int minWait, int maxWait)
    {
        Debug.Log("Starting");
        Vector3 newLocation = (currentDestination = pickRandomPoint());
        initalizeAtPoint(newLocation);
        StartCoroutine(move(newLocation, random.Next(minWait, maxWait)));
    }

    protected void initalizeAtPoint(Vector3 point)
    {
        Debug.Log("Making new orb");
        Instantiate(LocationPrefab, new Vector3(point.x, 0, point.z), LocationPrefab.transform.rotation);
    }

    public bool isCorrectLocation(Vector3 location)
    {
        return (location.x == currentDestination.x && location.z == currentDestination.z);
    }

    /*
     * Change so it cant repeat just in case
     */
    protected Vector3 pickRandomPoint()
    {
        return new Vector3(
            UnityEngine.Random.Range(waterBounds.min.x * trim, waterBounds.max.x * trim), 
            transform.position.y, 
            UnityEngine.Random.Range(waterBounds.min.z * trim, waterBounds.max.z * trim)
            );
    }
}
