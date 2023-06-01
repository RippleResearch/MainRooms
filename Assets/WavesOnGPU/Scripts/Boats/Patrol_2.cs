using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol_2 : PatrolBoat
{
    public override void Start()
    {
        waitMin = 0; waitMax = 10;
        setSpeed(5); setTurnSpeed(130);
        beginMove(waitMin, waitMax);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Locations") && isCorrectLocation(other.transform.position))
        {
            Destroy(other.gameObject);
            beginMove(waitMin, waitMax);
        }
    }
}
