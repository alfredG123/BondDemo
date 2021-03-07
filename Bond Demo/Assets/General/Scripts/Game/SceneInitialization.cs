using UnityEngine;

public class SceneInitialization : MonoBehaviour
{
    /// <summary>
    /// Create a game information object when the scene is loaded
    /// </summary>
    private void Start()
    {
        bool reset_game_info = false;

        // If the scene is the title scene, reset the game information object
        if (GeneralScene.GetCurrentScene() == GeneralScene.Scene.Title)
        {
            reset_game_info = true;
        }

        GameInformation.CreateInstance(reset_game_info);
    }
}
