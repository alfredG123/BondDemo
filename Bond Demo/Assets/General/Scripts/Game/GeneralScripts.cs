using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GeneralScripts
{
    public static void ReturnToStarterScene(string place_of_occurrence)
    {
        //Back to picking starter;
        SceneManager.LoadScene(0);

        //Add transition

        Debug.Log("There is an error in the script, " + place_of_occurrence);
    }

    public static Vector3 GetMousePositionInWorldSpace()
    {
        return (GetPositionInWorldSpace(Input.mousePosition));
    }

    public static Vector3 GetPositionInWorldSpace(Vector3 _position)
    {
        Vector2 world_position = Camera.main.ScreenToWorldPoint(_position);

        return (world_position);
    }

    public static void SetMainCameraPositionXYOnly(Vector3 position_to_set)
    {
        Vector3 position = Camera.main.transform.position;
        
        position.x = position_to_set.x;
        position.y = position_to_set.y;

        Camera.main.transform.position = position;
    }
}
