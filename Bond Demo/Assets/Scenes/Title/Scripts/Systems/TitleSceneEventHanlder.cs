using UnityEngine;

public class TitleSceneEventHanlder : MonoBehaviour
{
    /// <summary>
    /// Listen to the key press event, and act accordingly
    /// </summary>
    private void Update()
    {
        // Press Enter Key
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadPartnerSelectionScene();
        }

        // Press Backspace key
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
        GeneralScene.LoadScene(Scene.PartnerSelection);
    }

    /// <summary>
    ///  Close the application
    /// </summary>
    public void QuitApplication()
    {
        GeneralApplication.QuitGame();
    }
}
