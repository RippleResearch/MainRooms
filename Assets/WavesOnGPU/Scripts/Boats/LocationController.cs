using System.Collections;
using UnityEngine;

/// <summary>
/// Helper class that will automatically destory one of the boats locations
/// if it extends a certain time to live. 
/// </summary>
public class LocationController : MonoBehaviour
{

    private float timeToLive = 120; //default
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitToDestroy());
    }

    private IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);

        Debug.Log("LOCATION PREFAB WAS ALIVE TO LONG");
    }
}
