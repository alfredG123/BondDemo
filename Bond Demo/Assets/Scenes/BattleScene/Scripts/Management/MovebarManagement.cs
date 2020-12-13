using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovebarManagement
{
    private readonly List<GameObject> monster_list;
    private int current_monster = 0;

    public MovebarManagement()
    {
        monster_list = new List<GameObject>();
    }

    public void AddMonsterToFight(GameObject monster_to_add)
    {
        monster_list.Add(monster_to_add);
    }

    public GameObject GetFirstMonster()
    {
        GameObject monster_to_move = monster_list[current_monster];

        //Update index
        current_monster++;

        if (current_monster >= monster_list.Count)
        {
            current_monster = 0;
        }

        return (monster_to_move);
    }
}
