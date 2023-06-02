using System;
using System.Collections;
using UnityEngine;

public class ChaseBoat : BasePatrolBoat
{
    public Transform mainShip; //serves as temp destination of the ship we are chasing
    private bool chase = false;


    public override void Start()
    {
        base.Start();
        SetSpeed(10); SetTurnSpeed(120);

        Debug.Assert(mainShip != null);
    }
    /*
     * When using OnTrigger Enter make sure the
     * at least one object has a rigid body and the isKinematic setting
     * is true and at least one object has the onTrigger set to true.
     * If both onTriggers are on no event will be triggered. No event
     * will also be triggered if the rigid body is not kinematic.
     */
    public override void OnTriggerEnter(Collider other)
    {
        if (!chase && random.Next(0, 10) == 1)
        {
                InvokeRepeating("ChaseTarget", 0, .1f);
                chase = true; 
        }
        else if(chase && other.gameObject.Equals(mainShip.gameObject))
        {
                CancelInvoke();
                //Try to leave immediatly
                PickRandomPoint(); // Updates NavPath
                navAgent.SetPath(navPath);
                chase = false;
        }
        base.OnTriggerEnter(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(mainShip.gameObject))
        {
            MoveWithoutDestroy(waitMin, waitMax);
        }
    }

    /// <summary>
    /// Called through an invoke repeating so it will constantly 
    ///update its destination with the playable boats position
    /// </summary>
    public void ChaseTarget()
    {
        navAgent.SetDestination(mainShip.position);
    }

   
}
