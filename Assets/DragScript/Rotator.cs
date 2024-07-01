using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _speed = 30.0f; // Speed of rotation
    private Vector3 _lastMousePosition;
    private bool _isDragging = false;

    void Update()
    {
        HandleKeyboardInput();
        HandleMouseDrag();
    }

    private void HandleKeyboardInput()
    {
        Vector3 rotation = Vector3.zero;

        if (Input.GetKey(KeyCode.A)) 
            rotation = Vector3.up;
        else if (Input.GetKey(KeyCode.D)) 
            rotation = Vector3.down;

        transform.Rotate(rotation * _speed * Time.deltaTime);
    }

    private void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    _isDragging = true;
                    _lastMousePosition = Input.mousePosition;
                }
            }
        }

        if (Input.GetMouseButton(0) && _isDragging)
        {
            Vector3 deltaMousePosition = Input.mousePosition - _lastMousePosition;
            float rotationY = deltaMousePosition.x * _speed * Time.deltaTime;
            transform.Rotate(0, rotationY, 0);
            _lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
    }
}
