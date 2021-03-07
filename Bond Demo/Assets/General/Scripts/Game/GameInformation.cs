using UnityEngine;

public class GameInformation : MonoBehaviour
{
    /// <summary>
    /// Make the game object stay between different scenes
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Create a game information object if it is not in scene already
    /// </summary>
    public static void CreateInstance(bool reset)
    {
        GameObject game_information_object;

        // Find the game object in the scene
        game_information_object = GameObject.Find("GameInformation(Clone)");

        // If the game object is not in the scene, create a instance of the prefab from the Resources Folder
        if (game_information_object == null)
        {
            InstantiateGameInfo();
        }

        // If the game object is in the scene, and the reset flag is set, replace the existing game information
        else if (reset)
        {
            Destroy(game_information_object);

            InstantiateGameInfo();
        }
    }

    /// <summary>
    /// Create a instance of the game information object
    /// </summary>
    private static void InstantiateGameInfo()
    {
        string prefab_name_with_path;

        prefab_name_with_path = "Prefab/System/GameInformation";

        GameObject.Instantiate(Resources.Load<GameObject>(prefab_name_with_path));
    }
}
