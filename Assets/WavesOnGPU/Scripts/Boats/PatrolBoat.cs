using System;
using System.Collections;
using System.Collections.Generic;
/*using Unity.VisualScripting.ReorderableList;*/
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public abstract class PatrolBoat : MonoBehaviour
{

    public float speed;
    public float turnSpeed;
    protected NavMeshAgent myAgent;
    protected System.Random random;

    public GameObject[] locations;

    public Vector3 currentDestination;

    public void Start()
    {
        random = new System.Random(); 
        locations = GameObject.FindGameObjectsWithTag("Locations");
        myAgent = GetComponent<NavMeshAgent>();

        Debug.Assert(locations.Length != 0);
        Debug.Assert(myAgent != null); // make sure all boats have nav mesh       


        //Start movement and update currentDestination
        myAgent.SetDestination(currentDestination = pickRandomLocation());
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

    /*
     * Picks a random location for any of the values
     * within the locations array. We onyl check the x and z
     * value because we are moving across a 2D plane.
     */
    public Vector3 pickRandomLocation()
    {
        Vector3 nextlocation;
        do{
            var randomIndex = random.Next(0, locations.Length);
            nextlocation = locations[randomIndex].transform.position;
        } while (currentDestination.x == nextlocation.x && currentDestination.z == nextlocation.z);
        
        Debug.Assert(currentDestination != nextlocation);
        currentDestination = nextlocation;
        return new Vector3(nextlocation.x, transform.position.y, nextlocation.z);
    }
}
