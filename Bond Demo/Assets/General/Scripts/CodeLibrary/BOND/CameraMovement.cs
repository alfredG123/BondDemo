using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    private Vector3 _CameraPosition = Vector3.zero;
    private readonly float _CameraMoveSpeed = 10f;

    private bool _IsCameraMoveable = false;

    private bool _LimitIsSet = false;
    private float _LowerBoundX = 0f;
    private float _LowerBoundY = 0f;
    private float _UpperBoundX = 0f;
    private float _UpperBoundY = 0f;

    private readonly float _SmoothSpeed = 0.4f;
    private Vector3 _TargetPosition;

    private bool _ImmediateAction = false;
    private Vector3 _ImmediatePosition;

    private void Awake()
    {
        _TargetPosition = transform.position;
    }

    private void FixedUpdate()
    {
        _CameraPosition = transform.position;

        if (_IsCameraMoveable)
        {
            _TargetPosition = _CameraPosition;

            // Move the camera toward the top
            if (Input.GetKey(KeyCode.W))
            {
                _TargetPosition.y += _CameraMoveSpeed * Time.deltaTime;
            }

            // Move the camera toward the bottom
            if (Input.GetKey(KeyCode.S))
            {
                _TargetPosition.y -= _CameraMoveSpeed * Time.deltaTime;
            }

            // Move the camera toward the left
            if (Input.GetKey(KeyCode.A))
            {
                _TargetPosition.x -= _CameraMoveSpeed * Time.deltaTime;
            }

            // Move the camera toward the right
            if (Input.GetKey(KeyCode.D))
            {
                _TargetPosition.x += _CameraMoveSpeed * Time.deltaTime;
            }

            transform.position = _TargetPosition;
        }

        _CameraPosition.x = Vector2.Lerp(transform.position, _TargetPosition, _SmoothSpeed * Time.deltaTime).x;
        _CameraPosition.y = Vector2.Lerp(transform.position, _TargetPosition, _SmoothSpeed * Time.deltaTime).y;

        if (_ImmediateAction)
        {
            // limit the x and the y
            if (_LimitIsSet)
            {
                transform.position = new Vector3(Mathf.Clamp(_ImmediatePosition.x, _LowerBoundX, _UpperBoundX), Mathf.Clamp(_ImmediatePosition.y, _LowerBoundY, _UpperBoundY), transform.position.z);
            }
            else
            {
                transform.position = _CameraPosition;
            }

            _ImmediateAction = false;
        }
        else
        {
            // limit the x and the y
            if (_LimitIsSet)
            {
                transform.position = new Vector3(Mathf.Clamp(_CameraPosition.x, _LowerBoundX, _UpperBoundX), Mathf.Clamp(_CameraPosition.y, _LowerBoundY, _UpperBoundY), transform.position.z);
            }
            else
            {
                transform.position = _CameraPosition;
            }
        }
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

    public void SetTargetPosition(Vector2 target_position)
    {
        _TargetPosition = target_position;
    }

    /// <summary>
    /// Set the main camera's position without changing the z-coordinate
    /// </summary>
    /// <param name="position_to_set"></param>
    public void SetMainCameraPositionXYOnly(Vector3 position_to_set)
    {
        Vector3 camera_position;

        camera_position = Camera.main.transform.position;
        camera_position.x = position_to_set.x;
        camera_position.y = position_to_set.y;

        transform.position = camera_position;

        _TargetPosition = position_to_set;
    }

    /// <summary>
    /// Set the main camera's position without changing the z-coordinate
    /// </summary>
    /// <param name="position_to_set"></param>
    public void SetMainCameraPositionXYOnlyImmediate(Vector3 position_to_set)
    {
        Vector3 camera_position;

        camera_position = Camera.main.transform.position;
        camera_position.x = position_to_set.x;
        camera_position.y = position_to_set.y;

        transform.position = camera_position;

        _ImmediateAction = true;
        _ImmediatePosition = position_to_set;
        _TargetPosition = position_to_set;
    }
}
