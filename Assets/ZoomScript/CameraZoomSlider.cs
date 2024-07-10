using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraZoomSlider : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Slider _zoomSlider;
    [SerializeField] private float _minZoom = 5.0f;
    [SerializeField] private float _maxZoom = 60.0f;

    private void Start()
    {
        // Configure the slider
        _zoomSlider.minValue = _minZoom;
        _zoomSlider.maxValue = _maxZoom;

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
