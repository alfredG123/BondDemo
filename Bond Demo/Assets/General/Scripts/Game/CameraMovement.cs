using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    private Vector3 _camera_position;
    private float _camera_move_speed = 10f;
    
    #if TEMPORARY_DEACTIVATE
    private float _distance_to_border = 10f;
    #endif

    private void Update()
    {
        _camera_position = transform.position;


        // Keyboard controls


        // Move the camera toward the top
        if (Input.GetKey(KeyCode.W))
        {
            _camera_position.y += _camera_move_speed * Time.deltaTime;
        }

        // Move the camera toward the bottom
        if (Input.GetKey(KeyCode.S))
        {
            _camera_position.y -= _camera_move_speed * Time.deltaTime;
        }

        // Move the camera toward the left
        if (Input.GetKey(KeyCode.A))
        {
            _camera_position.x -= _camera_move_speed * Time.deltaTime;
        }

        // Move the camera toward the right
        if (Input.GetKey(KeyCode.D))
        {
            _camera_position.x += _camera_move_speed * Time.deltaTime;
        }


#if TEMPORARY_DEACTIVATE
        // Mouse controls


        // Move the camera toward the top, if the mouse is close or exceed the top border of the screen
        if (Input.mousePosition.y >= Screen.height - _distance_to_border)
        {
            _camera_position.y += _camera_move_speed * Time.deltaTime;
        }

        // Move the camera toward the top, if the mouse is close or exceed the bottom border of the screen
        if (Input.mousePosition.y <= _distance_to_border)
        {
            _camera_position.y -= _camera_move_speed * Time.deltaTime;
        }

        // Move the camera toward the top, if the mouse is close or exceed the left border of the screen
        if (Input.mousePosition.x <= _distance_to_border)
        {
            _camera_position.x -= _camera_move_speed * Time.deltaTime;
        }

        // Move the camera toward the top, if the mouse is close or exceed the right border of the screen
        if (Input.mousePosition.x >= Screen.width - _distance_to_border)
        {
            _camera_position.x += _camera_move_speed * Time.deltaTime;
        }
#endif

        //use clamp to limit the x and the y
        //mathf(value, min, max)

        transform.position = _camera_position;
    }
}
