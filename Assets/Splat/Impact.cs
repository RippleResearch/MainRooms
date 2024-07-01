using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    public GameObject impact;

    void OnCollisionEnter(Collision collision)
    {
        // Only create an impact mark if the collided object is tagged "Wall"
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (impact != null) // Check if the impact prefab is not null
            {
                // Instantiate a new game object at the point of impact
                GameObject newImpact = Instantiate(impact, collision.GetContact(0).point + collision.GetContact(0).normal * 0.001F, Quaternion.LookRotation(collision.GetContact(0).normal));
                ImpactManager.Instance.RegisterImpact(newImpact);

                // Destroy the instantiated object, not the prefab
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Impact prefab is not assigned.");
            }
        }
    }
}

public class ImpactManager : MonoBehaviour
{
    public static ImpactManager Instance { get; private set; }

    private List<GameObject> impactMarks = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterImpact(GameObject impact)
    {
        impactMarks.Add(impact);
    }

    public void ClearImpacts()
    {
        foreach (var impact in impactMarks)
        {
            if (impact != null)
            {
                Destroy(impact);
            }
        }
        impactMarks.Clear();
    }
}

