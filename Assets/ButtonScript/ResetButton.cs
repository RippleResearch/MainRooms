using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ClearImpacts);
    }

    private void ClearImpacts()
    {
        ImpactManager.Instance.ClearImpacts();
    }
}

