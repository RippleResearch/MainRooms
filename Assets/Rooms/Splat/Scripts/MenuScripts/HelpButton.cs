using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{
    public GameObject helpTextObject; // Assign the HelpText GameObject in the Inspector
    public string helpMessage = "Here is how to play the game and customize the boat...";

    void Start()
    {
        // Add listener to the button
        GetComponent<Button>().onClick.AddListener(ShowHelp);

        // Ensure the help text is hidden initially
        if (helpTextObject != null)
        {
            helpTextObject.SetActive(false);
        }
        else
        {
            Debug.LogError("HelpTextObject is not assigned in the Inspector.");
        }
    }

    void ShowHelp()
    {
        if (helpTextObject != null)
        {
            // Activate the help text object
            helpTextObject.SetActive(true);

            // Set the help text
            Text helpTextComponent = helpTextObject.GetComponent<Text>();
            if (helpTextComponent != null)
            {
                helpTextComponent.text = helpMessage;
            }
            else
            {
                Debug.LogError("No Text component found on HelpTextObject.");
            }
        }
        else
        {
            Debug.LogError("HelpTextObject is not assigned in the Inspector.");
        }
    }
}
