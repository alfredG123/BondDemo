using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralScripts
{
    public static void CreateDefaultGameManager()
    {
        GameObject game_manager = new GameObject("GeneralGameManager");
        game_manager.AddComponent<GameManagement>();
        game_manager.AddComponent<PlayerManagement>();
        game_manager.AddComponent<MonsterManagement>();
        game_manager.AddComponent<LevelManagement>();
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
