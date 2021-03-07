using UnityEngine;

public class TitleEventHandlers : MonoBehaviour
{
    /// <summary>
    /// Listen to the key press event, and act accordingly
    /// </summary>
    private void Update()
    {
        // If Enter Key is pressed, load the next scene
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadPartnerSelectionScene();
        }

        // If Backspace key is pressed, quit the game
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            QuitApplication();
        }
    }

    /// <summary>
    /// Load the scene for picking the partner spirit
    /// </summary>
    public void LoadPartnerSelectionScene()
    {
        GeneralScene.LoadScene(GeneralScene.Scene.PartnerSelection);
    }

    /// <summary>
    ///  Close the application
    /// </summary>
    public void QuitApplication()
    {
        GeneralApplication.QuitGame();
    }
}
