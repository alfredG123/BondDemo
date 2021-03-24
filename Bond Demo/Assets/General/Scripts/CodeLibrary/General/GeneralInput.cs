using UnityEngine;

public static class GeneralInput
{
    /// <summary>
    /// Get the mouse position in the world space
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetMousePositionInWorldSpace()
    {
        return (ConvertScreenToWorldPosition(Input.mousePosition));
    }

    /// <summary>
    /// Get the mouse position in the screen space
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetMousePositionOnScreen()
    {
        return (Input.mousePosition);
    }

    /// <summary>
    /// Convert the screen position to the world position
    /// </summary>
    /// <param name="screen_position_to_convert"></param>
    /// <returns></returns>
    public static Vector3 ConvertScreenToWorldPosition(Vector3 screen_position_to_convert)
    {
        Vector2 world_position;

        world_position = Camera.main.ScreenToWorldPoint(screen_position_to_convert);

        return (world_position);
    }

    /// <summary>
    /// Convert the world position to the screen position
    /// </summary>
    /// <param name="world_position_to_convert"></param>
    /// <returns></returns>
    public static Vector3 ConvertWorldToScreenPosition(Vector3 world_position_to_convert)
    {
        Vector2 screen_position;

        screen_position = Camera.main.WorldToScreenPoint(world_position_to_convert);

        return (screen_position);
    }

    /// <summary>
    /// If there is anything colliding with the mouse, return it
    /// </summary>
    /// <returns></returns>
    public static GameObject GetGameObjectAtMousePosition()
    {
        Collider2D game_object_detector;
        GameObject game_object_to_return = null;
        float radius = 0.01f;

        // Check the collision at the mouse position with a radius of 0.01
        game_object_detector = Physics2D.OverlapCircle(GetMousePositionInWorldSpace(), radius);

        // If the collider is not null, return it
        if (game_object_detector != null)
        {
            game_object_to_return = game_object_detector.gameObject;
        }

        return (game_object_to_return);
    }
}
