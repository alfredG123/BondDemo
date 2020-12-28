using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManagement : MonoBehaviour
{
    #region Button Handlers
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
    #endregion
}
