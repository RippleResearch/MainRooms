using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBoat : PatrolBoat
{
    public Transform target; //serves as temp destination of the ship we are chasing
    /*
     * When using OnTrigger Enter make sure the
     * at least one object has a rigid body and the isKinematic setting
     * is true and at least one object has the onTrigger set to true.
     * If both onTriggers are on no event will be triggered. No event
     * will also be triggered if the rigid body is not kinematic.
     */
    public void OnTriggerEnter(Collider other)
    {
        if (random.Next(0, 50) == 1)
        {
            InvokeRepeating("chaseBoat", 1f, .1f);
        }
        if (other.gameObject.Equals(target.gameObject))
        {
            Debug.Log("hitting ship");
            CancelInvoke();
        }


        StartCoroutine(move(pickRandomLocation(), random.Next(1,10)));
    }
    public void chaseBoat()
    {
        myAgent.SetDestination(target.position);
    }
}
