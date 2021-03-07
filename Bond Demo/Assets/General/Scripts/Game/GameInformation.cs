using UnityEngine;

public class GameInformation : MonoBehaviour
{
    /// <summary>
    /// Make the game object stay between different scenes
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (GeneralScene.GetCurrentScene() != GeneralScene.Scene.Title)
        {
            SettingPanelDisplay.CreateSettingPanel();
        }
    }

    /// <summary>
    /// Create a game information object if it is not in scene already
    /// </summary>
    public static void CreateInstance()
    {
        string prefab_name_with_path;
        GameObject game_information_object;

        // Find the game object in the scene
        game_information_object = GameObject.Find("GameInformation");

        // If the game object is not in the scene, create a instance of the prefab from the Resources Folder
        if (game_information_object == null)
        {
            prefab_name_with_path = "Prefab/System/GameInformation";

            GameObject.Instantiate(Resources.Load<GameObject>(prefab_name_with_path));
        }
    }
}
