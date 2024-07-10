using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HamburgerMenu : MonoBehaviour
{
    public GameObject menuPanel; // Assign the MenuPanel in the Inspector
    public GameObject helpTextObject; // Assign the HelpText GameObject in the Inspector
    public Button zoomButton; // Assign the Zoom Button in the Inspector
    public Slider zoomSlider; // Assign the Zoom Slider in the Inspector
    public Camera mainCamera; // Assign the Main Camera in the Inspector

    [SerializeField] private float minZoom = 5.0f;
    [SerializeField] private float maxZoom = 60.0f;

    void Start()
    {
        // Ensure the menu is hidden initially
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("MenuPanel is not assigned in the Inspector.");
        }

        // Add listener to the menu button (assumed to be this GameObject's button)
        Button menuButton = GetComponent<Button>();
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(ToggleMenu);
        }
        else
        {
            Debug.LogError("Button component is not found on this GameObject.");
        }

        // Ensure the help text is hidden initially
        if (helpTextObject != null)
        {
            helpTextObject.SetActive(false);
        }
        else
        {
            Debug.LogError("HelpTextObject is not assigned in the Inspector.");
        }

        // Ensure the slider is hidden initially
        if (zoomSlider != null)
        {
            zoomSlider.gameObject.SetActive(false);
            zoomSlider.minValue = minZoom;
            zoomSlider.maxValue = maxZoom;
            zoomSlider.onValueChanged.AddListener(OnZoomSliderValueChanged);

            // Initialize the slider value
            if (mainCamera != null)
            {
                if (mainCamera.orthographic)
                {
                    zoomSlider.value = mainCamera.orthographicSize;
                }
                else
                {
                    zoomSlider.value = mainCamera.fieldOfView;
                }
            }
            else
            {
                Debug.LogError("Main Camera is not assigned in the Inspector.");
            }
        }
        else
        {
            Debug.LogError("Zoom Slider is not assigned in the Inspector.");
        }

        // Add listener to the zoom button
        if (zoomButton != null)
        {
            zoomButton.onClick.AddListener(ToggleZoomSlider);
        }
        else
        {
            Debug.LogError("Zoom Button is not assigned in the Inspector.");
        }
    }

    void ToggleMenu()
    {
        if (menuPanel != null)
        {
            // Toggle the menu panel
            bool isActive = !menuPanel.activeSelf;
            menuPanel.SetActive(isActive);

            // Hide the help text if the menu is being hidden
            if (!isActive && helpTextObject != null)
            {
                helpTextObject.SetActive(false);
            }
        }
    }

    void ToggleZoomSlider()
    {
        // Toggle the zoom slider
        if (zoomSlider != null)
        {
            zoomSlider.gameObject.SetActive(!zoomSlider.gameObject.activeSelf);
        }
    }

    void OnZoomSliderValueChanged(float value)
    {
        if (mainCamera != null)
        {
            if (mainCamera.orthographic)
            {
                mainCamera.orthographicSize = value;
            }
            else
            {
                mainCamera.fieldOfView = value;
            }
        }
    }
}
