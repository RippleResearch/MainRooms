using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBoat : PatrolBoat
{
    public Transform target; //serves as temp destination of the ship we are chasing

    private bool chase = false;

    public override void Start()
    {
        waitMin = 0; waitMax = 10;
        setSpeed(4); setTurnSpeed(120);
        beginMove(waitMin, waitMax);
    }
    /*
     * When using OnTrigger Enter make sure the
     * at least one object has a rigid body and the isKinematic setting
     * is true and at least one object has the onTrigger set to true.
     * If both onTriggers are on no event will be triggered. No event
     * will also be triggered if the rigid body is not kinematic.
     */
    public void OnTriggerEnter(Collider other)
    { 
        if (!chase)
        {
            if (random.Next(0, 50) == 1)
            {
                InvokeRepeating("chaseBoat", 1f, .1f);
                chase = true;
            }
        }
        else if (chase && other.gameObject.Equals(target.gameObject))
        {
            CancelInvoke();
            chase = false;
            beginMove(waitMin,waitMax);
        }

        if (other.gameObject.tag.Equals("Locations") && isCorrectLocation(other.transform.position))  
        {
            Destroy(other.gameObject);
            beginMove(waitMin, waitMax);
        }
            
    }
    /*
     * Called through an invoke repeating so it will constantly 
     * update its destination with the playable boats position
     */
    public void chaseBoat()
    {
        myAgent.SetDestination(target.position);
    }

   
}
