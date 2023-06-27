using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePatrolBoat : AIBoat
{ 
    public virtual void Start()
    {
        waitMin = 3; waitMax = 3;
        SetSpeed(10); SetTurnSpeed(130);
        SetRandomDestination(waitMin, waitMax);
    }

    /// <summary>
    /// Currently checks if we have reached the desginated location
    /// and if so we move the target and the boats new destination.
    /// We also set the timeInTrigger field to the current world time.
    /// </summary>
    /// <param name="other"></param>
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(targetObject))
        { 
            SetRandomDestination(waitMin, waitMax);
        }
    }
}
