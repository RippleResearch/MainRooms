using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.WSA;

public class ChaseBoat : BasePatrolBoat
{
    public Transform mainShip; //serves as temp destination of the ship we are chasing

    private bool chase = false;


    public override void Start()
    {
        base.Start();
        SetSpeed(2); SetTurnSpeed(120);
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
        if (!chase)
        {
            if (random.Next(0, 4) == 1)
            {
                InvokeRepeating("chaseBoat", 1f, .1f);
                chase = true;
            }
        }
        else
        {

        }

        base.OnTriggerEnter(other);
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
