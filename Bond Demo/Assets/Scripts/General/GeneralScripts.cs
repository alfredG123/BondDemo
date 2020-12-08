using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralScripts
{
    public static GameObject CreateDefaultGameManager(GameObject game_manager_prefab)
    {

        GameObject game_manager = GameObject.Instantiate(game_manager_prefab);
        game_manager.GetComponent<PlayerManagement>().SetLinkedMonster(new BaseMonsterInfo());

        return (game_manager);
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
