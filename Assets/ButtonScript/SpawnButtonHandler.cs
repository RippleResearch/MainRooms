using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButtonHandler : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if (Spawner.Instance != null)
        {
            Spawner.Instance.SpawnNewObject();
        }
        else
        {
            Debug.LogWarning("Spawner instance is not set.");
        }
    }
}
