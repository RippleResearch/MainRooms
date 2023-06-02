using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePatrolBoat : AIBoat
{
    protected float timeInTrigger;
    public virtual void Start()
    {
        waitMin = 3; waitMax = 3;
        SetSpeed(10); SetTurnSpeed(130);
        MoveWithoutDestroy(waitMin, waitMax);
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
            MoveWithoutDestroy(waitMin, waitMax);
            timeInTrigger = Time.time; //Current time we stay in trigger
        }
    }

    /// <summary>
    /// OnTriggerStay is called almost every frame it is in the 
    /// same collider. This currently checks if we are in the 
    /// designated collider and if we have been there for longer
    /// than the maximum wait time. 
    /// </summary>
    /// <param name="other"></param>
    public virtual void OnTriggerStay(Collider other)
    {
        Debug.Log("IN");
        if(other.gameObject.Equals(targetObject))
        {
            float timeStayInTrigger = Time.time - timeInTrigger;
            if (timeStayInTrigger > waitMax)
            {
                Debug.Break();
                MoveWithoutDestroy(waitMin, waitMax);
            }
        }
       
    }
}
