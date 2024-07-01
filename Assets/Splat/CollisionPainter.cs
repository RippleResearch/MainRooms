using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPainter : MonoBehaviour
{
    public Color paintColor;
    public float radius = 1;
    public float strength = 1;
    public float hardness = 1;

    public GameObject impact;  // Impact prefab
    public GameObject SplatMark; // SplatMark prefab

    private bool isThrown = false; // Flag to check if the object has been thrown

    private void OnCollisionEnter(Collision collision)
    {
        if (isThrown)
        {
            try
            {
                // Compare tag to make sure you don't create mark on something you can't hit
                if (!collision.gameObject.CompareTag("Ball"))
                {
                    // Instantiate a new impact object at the point of impact
                    Vector3 impactPosition = collision.GetContact(0).point + collision.GetContact(0).normal * 0.01F;
                    Quaternion impactRotation = Quaternion.FromToRotation(Vector3.up, collision.GetContact(0).normal);
                    GameObject NewImpact = Instantiate(impact, impactPosition, impactRotation);

                    // Destroy the new impact object after 10 seconds
                    Destroy(NewImpact, 10f);

                    // Instantiate a new splat mark at the point of impact
                    GameObject NewSplatMark = Instantiate(SplatMark, impactPosition, impactRotation);

                    // Destroy the new splat mark object after 10 seconds
                    Destroy(NewSplatMark, 10f);

                    Debug.Log("Impact and splat mark created at position: " + impactPosition);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error in OnCollisionEnter: " + e.Message);
                Debug.LogError(e.StackTrace);
            }
        }
    }

    // Method to mark the object as thrown
    public void MarkAsThrown()
    {
        isThrown = true;
        Debug.Log("Object marked as thrown.");
    }
}
