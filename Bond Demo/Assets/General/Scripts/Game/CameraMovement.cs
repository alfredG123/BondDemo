using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    private Vector3 _CameraPosition = Vector3.zero;
    private float _CameraMoveSpeed = 10f;

    private bool _IsCameraMoveable = false;

    private bool _LimitIsSet = false;
    private float _LowerBoundX = 0f;
    private float _LowerBoundY = 0f;
    private float _UpperBoundX = 0f;
    private float _UpperBoundY = 0f;

    private void Update()
    {
        _CameraPosition = transform.position;

        if (_IsCameraMoveable)
        {
            // Move the camera toward the top
            if (Input.GetKey(KeyCode.W))
            {
                _CameraPosition.y += _CameraMoveSpeed * Time.deltaTime;
            }

            // Move the camera toward the bottom
            if (Input.GetKey(KeyCode.S))
            {
                _CameraPosition.y -= _CameraMoveSpeed * Time.deltaTime;
            }

            // Move the camera toward the left
            if (Input.GetKey(KeyCode.A))
            {
                _CameraPosition.x -= _CameraMoveSpeed * Time.deltaTime;
            }

            // Move the camera toward the right
            if (Input.GetKey(KeyCode.D))
            {
                _CameraPosition.x += _CameraMoveSpeed * Time.deltaTime;
            }
        }

        // limit the x and the y
        if (_LimitIsSet)
        {
            _CameraPosition.x = Mathf.Clamp(_CameraPosition.x, _LowerBoundX, _UpperBoundX);

            _CameraPosition.y = Mathf.Clamp(_CameraPosition.y, _LowerBoundY, _UpperBoundY);
        }

        transform.position = _CameraPosition;
    }

    public void EnableCameraMovement(bool is_enable)
    {
        _IsCameraMoveable = is_enable;
    }

    public void EnableCameraBound(bool is_enable)
    {
        _LimitIsSet = is_enable;
    }

    public void SetCameraBound(float lower_x, float lower_y, float upper_x, float upper_y)
    {
        Camera camera = Camera.main;
        float height = camera.orthographicSize;
        float width = height * camera.aspect;

        _LowerBoundX = lower_x + width;
        _LowerBoundY = lower_y + height;
        _UpperBoundX = upper_x - width;
        _UpperBoundY = upper_y - height;
    }
}
