using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    // Make the game object stay between different scenes
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
