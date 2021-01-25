using UnityEngine;

public class TitleKeyEventHanlder : MonoBehaviour
{
    /// <summary>
    /// Listen to the key press event, and act accordingly
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Load the next scene
            LoadPartnerSelectionScene();
        }
        else if(Input.GetKeyDown(KeyCode.Backspace))
        {
            QuitGame();
        }
    }

    /// <summary>
    /// Load the scene for picking the partner spirit
    /// </summary>
    public void LoadPartnerSelectionScene()
    {
        General.LoadScene(TypeScene.PartnerSelection);
    }

    /// <summary>
    ///  Exit the application
    /// </summary>
    public void QuitGame()
    {
        General.QuitGame();
    }
}
