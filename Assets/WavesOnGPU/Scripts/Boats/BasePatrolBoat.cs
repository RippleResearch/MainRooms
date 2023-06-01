using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePatrolBoat : AIBoat
{
    public virtual void Start()
    {
        waitMin = 0; waitMax = 0;
        SetSpeed(10); SetTurnSpeed(130);
        BeginMove(waitMin, waitMax);
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(targetObject))
        {
            MoveWithoutDestroy(waitMin, waitMax);
        }
    }
}
