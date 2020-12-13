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

    public static Vector2 GetMousePositionInWorldSpace()
    {
        return (GetPositionInWorldSpace(Input.mousePosition));
    }

    public static Vector2 GetPositionInWorldSpace(Vector2 _position)
    {
        Vector2 world_position = Camera.main.ScreenToWorldPoint(_position);

        return (world_position);
    }
}
