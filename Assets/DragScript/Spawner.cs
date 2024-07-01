using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Vector3 SpawnPos;
    public GameObject[] spawnObjects; // Array to hold different spawn objects
    private float newSpawnDuration = 1f;

    #region Singleton

    public static Spawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        SpawnPos = transform.position;
    }

    public void SpawnNewObject()
    {
        if (spawnObjects.Length == 0) return; // Check if there are objects to spawn

        // Randomly select an object from the array
        int randomIndex = Random.Range(0, spawnObjects.Length);
        GameObject randomObject = spawnObjects[randomIndex];

        if (randomObject != null) // Check if the object is not null
        {
            // Adjust spawn position to avoid immediate collision
            Vector3 adjustedSpawnPos = SpawnPos + new Vector3(0, 1, 0); // Example adjustment

            // Instantiate the selected object
            Instantiate(randomObject, adjustedSpawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Spawn object is null at index: " + randomIndex);
        }
    }

    public void NewSpawnRequest()
    {
        Invoke("SpawnNewObject", newSpawnDuration);
    }
}
