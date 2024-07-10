using UnityEngine;
using UnityEngine.UI;

public class ZoomControl : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Slider _zoomSlider;
    [SerializeField] private Button _zoomButton;
    [SerializeField] private float _minZoom = 5.0f;
    [SerializeField] private float _maxZoom = 60.0f;

    private void Start()
    {
        // Debug logs to check if the references are assigned
        if (_camera == null)
        {
            Debug.LogError("Camera is not assigned in the Inspector.");
            return;
        }
        if (_zoomSlider == null)
        {
            Debug.LogError("Zoom Slider is not assigned in the Inspector.");
            return;
        }
        if (_zoomButton == null)
        {
            Debug.LogError("Zoom Button is not assigned in the Inspector.");
            return;
        }

        // Configure the slider
        _zoomSlider.minValue = _minZoom;
        _zoomSlider.maxValue = _maxZoom;

        // Attach the button click event
        _zoomButton.onClick.AddListener(ToggleZoomSlider);

        // Attach the slider value change event
        _zoomSlider.onValueChanged.AddListener(OnZoomSliderValueChanged);

        // Initialize the slider value
        if (_camera.orthographic)
        {
            _zoomSlider.value = _camera.orthographicSize;
        }
        else
        {
            _zoomSlider.value = _camera.fieldOfView;
        }

        // Initially hide the slider
        _zoomSlider.gameObject.SetActive(false);
    }

    private void ToggleZoomSlider()
    {
        _zoomSlider.gameObject.SetActive(!_zoomSlider.gameObject.activeSelf);
    }

    private void OnZoomSliderValueChanged(float value)
    {
        if (_camera.orthographic)
        {
            _camera.orthographicSize = value;
        }
        else
        {
            _camera.fieldOfView = value;
        }
    }
}
