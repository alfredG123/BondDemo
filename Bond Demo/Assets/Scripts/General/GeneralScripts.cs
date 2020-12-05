using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralScripts
{
    public static Vector3 GetMousePositionIn2DWorld()
    {
        return (GetMousePositionIn2DWorld(Camera.main));
    }

    public static Vector3 GetMousePositionIn2DWorld(Camera camera)
    {
        Vector3 world_position = GetPositionIn3DWorld(Input.mousePosition, camera);
        world_position.z = 0;

        return (world_position);
    }

    public static Vector3 GetPositionIn2DWorld(Vector3 position)
    {
        return (GetPositionIn2DWorld(position, Camera.main));
    }

    public static Vector3 GetPositionIn2DWorld(Vector3 position, Camera camera)
    {
        Vector3 world_position = GetPositionIn3DWorld(position, camera);
        world_position.z = 0;

        return (world_position);
    }

    public static Vector3 GetPositionIn3DWorld(Vector3 position)
    {
        return (GetPositionIn3DWorld(position, Camera.main));
    }

    public static Vector3 GetPositionIn3DWorld(Vector3 position, Camera camera)
    {
        return (camera.ScreenToWorldPoint(position));
    }

    public static Vector3 GetPositionInScreen(Vector3 position)
    {
        return (GetPositionInScreen(position, Camera.main));
    }

    public static Vector3 GetPositionInScreen(Vector3 position, Camera camera)
    {
        return (camera.WorldToScreenPoint(position));
    }
}
