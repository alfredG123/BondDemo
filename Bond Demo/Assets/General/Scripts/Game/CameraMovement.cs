using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    private Vector3 position;
    private float speed = 10f;
    //private float thinkness = 10f;
    
    private void Update()
    {
        position = transform.position;

        if (Input.GetKey("s"))
        {
            position.y -= speed * Time.deltaTime;
        }

        if (Input.GetKey("w"))
        {
            position.y += speed * Time.deltaTime;
        }

        if (Input.GetKey("a"))
        {
            position.x -= speed * Time.deltaTime;
        }

        if (Input.GetKey("d"))
        {
            position.x += speed * Time.deltaTime;
        }

        //if (Input.mousePosition.y >= Screen.height - thinkness)
        //{
        //    position.y += speed * Time.deltaTime;
        //}

        //if (Input.mousePosition.y <= thinkness)
        //{
        //    position.y -= speed * Time.deltaTime;
        //}

        //use clamp to limit the x and the y
        //mathf(value, min, max)

        transform.position = position;
    }
}
