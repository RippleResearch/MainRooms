using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol_2 : PatrolBoat
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Locations") && isCorrectLocation(other.transform.position))
        {
            Debug.Log("Destroying target and moving");
            Destroy(other.gameObject);
            beginMove(0, 10);
        }
    }
}
