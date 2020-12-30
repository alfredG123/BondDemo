using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManagement : MonoBehaviour
{
    private void Update()
    {
        // Listen to keyboard input
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Load the next scene
            StartGame();
        }
        else if(Input.GetKeyDown(KeyCode.Backspace))
        {
            // exit the application
            QuitGame();
        }
    }

    // Load the scene for picking a starter
    public void StartGame()
    {
        GeneralScripts.LoadScene(TypeScene.StartersSelection);
    }

    // Exit the application
    public void QuitGame()
    {
        GeneralScripts.QuitGame();
    }
}
