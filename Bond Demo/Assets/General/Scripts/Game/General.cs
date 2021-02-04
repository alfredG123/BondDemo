using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class General
{
    #region GAME
    /// <summary>
    /// Load the title scene, and report errors in the console 
    /// </summary>
    /// <param name="place_of_occurrence"></param>
    /// <param name="additional_message"></param>
    public static void ReturnToTitleSceneForErrors(string place_of_occurrence, string additional_message)
    {
        // Add some transitions

        // Load the title screen
        SceneManager.LoadScene((int)TypeScene.Title);

        // Report the bug in the console
        Debug.LogError("There is an error in the script, " + place_of_occurrence);
        Debug.LogError(additional_message);
    }

    /// <summary>
    /// Load the specific scene
    /// </summary>
    /// <param name="scene_to_load"></param>
    public static void LoadScene(TypeScene scene_to_load)
    {
        SceneManager.LoadScene((int)scene_to_load);
    }

    /// <summary>
    /// Exit the application
    /// </summary>
    public static void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region INPUT
    /// <summary>
    /// Get the mouse position in the world space
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetMousePositionInWorldSpace()
    {
        return (ConvertScreenToWorldPosition(Input.mousePosition));
    }

    /// <summary>
    /// Convert the screen position to the world position
    /// </summary>
    /// <param name="screen_position_to_convert"></param>
    /// <returns></returns>
    public static Vector3 ConvertScreenToWorldPosition(Vector3 screen_position_to_convert)
    {
        Vector2 world_position = Camera.main.ScreenToWorldPoint(screen_position_to_convert);

        return (world_position);
    }

    /// <summary>
    /// Convert the world position to the screen position
    /// </summary>
    /// <param name="world_position_to_convert"></param>
    /// <returns></returns>
    public static Vector3 ConvertWorldToScreenPosition(Vector3 world_position_to_convert)
    {
        Vector2 screen_position = Camera.main.WorldToScreenPoint(world_position_to_convert);

        return (screen_position);
    }

    /// <summary>
    /// Set the main camera's position without changing the z-coordinate
    /// </summary>
    /// <param name="position_to_set"></param>
    public static void SetMainCameraPositionXYOnly(Vector3 position_to_set)
    {
        Vector3 camera_position = Camera.main.transform.position;
        
        camera_position.x = position_to_set.x;
        camera_position.y = position_to_set.y;

        Camera.main.transform.position = camera_position;
    }

    /// <summary>
    /// If there is anything colliding with the mouse, return it
    /// </summary>
    /// <returns></returns>
    public static GameObject GetGameObjectAtMousePosition()
    {
        // Check collision at mouse position
        Collider2D game_object_collided_with_mouse = Physics2D.OverlapCircle(GetMousePositionInWorldSpace(), 0.01f);

        // If the collider is not null, return it
        if (game_object_collided_with_mouse != null)
        {
            return (game_object_collided_with_mouse.gameObject);
        }

        return (null);
    }
    #endregion

    #region TEXT
    /// <summary>
    /// Modified the text of the text component
    /// </summary>
    /// <param name="text_object"></param>
    /// <param name="text_to_set"></param>
    public static void SetText(GameObject text_object, string text_to_set)
    {
        Text text_component = text_object.GetComponent<Text>();

        // Error handling
        if (text_component == null)
        {
            ReturnToTitleSceneForErrors("General.SetText", "game_object does not have the required component");
        }

        text_component.text = text_to_set;
    }
    #endregion
}
