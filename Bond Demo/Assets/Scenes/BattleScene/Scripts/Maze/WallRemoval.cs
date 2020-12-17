using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRemoval : MonoBehaviour
{
    private float wait_to_destory = 2f;

    private void Awake()
    {
        Destroy(gameObject, wait_to_destory);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(collision.gameObject);
        }
    }
}
