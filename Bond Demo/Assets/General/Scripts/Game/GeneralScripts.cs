using UnityEngine;
using UnityEngine.SceneManagement;

public static class GeneralScripts
{
    // Load the title scene, and report errors in the console
    public static void ReturnToTitleSceneForErrors(string place_of_occurrence, string additional_message)
    {
        // Add some transitions

        // Load the title screen
        SceneManager.LoadScene((int)TypeScene.Title);

        // Report the bug in the console
        Debug.LogError("There is an error in the script, " + place_of_occurrence);
        Debug.LogError(additional_message);
    }

    // Report errors in the console
    public static void ReportErrors(string place_of_occurrence, string additional_message)
    {
        // Report the bug in the console
        Debug.LogError("There is an error in the script, " + place_of_occurrence);
        Debug.LogError(additional_message);
    }

    // Load the specific scene
    public static void LoadScene(TypeScene scene_to_load)
    {
        SceneManager.LoadScene((int)scene_to_load);
    }

    // Exit the application
    public static void QuitGame()
    {
        Application.Quit();
    }

    // Get the mouse position in the world space
    public static Vector3 GetMousePositionInWorldSpace()
    {
        return (ConvertScreenToWorldPosition(Input.mousePosition));
    }

    // Convert the screen position to the world position
    public static Vector3 ConvertScreenToWorldPosition(Vector3 screen_position_to_convert)
    {
        Vector2 world_position = Camera.main.ScreenToWorldPoint(screen_position_to_convert);

        return (world_position);
    }

    // Convert the world position to the screen position
    public static Vector3 ConvertWorldToScreenPosition(Vector3 world_position_to_convert)
    {
        Vector2 screen_position = Camera.main.WorldToScreenPoint(world_position_to_convert);

        return (screen_position);
    }

    // Set the main camera's position without changing the z-coordinate
    public static void SetMainCameraPositionXYOnly(Vector3 position_to_set)
    {
        Vector3 camera_position = Camera.main.transform.position;
        
        camera_position.x = position_to_set.x;
        camera_position.y = position_to_set.y;

        Camera.main.transform.position = camera_position;
    }

    // If there is anything colliding with the mouse, return it
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
}
