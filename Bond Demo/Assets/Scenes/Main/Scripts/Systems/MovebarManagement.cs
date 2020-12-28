using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovebarManagement
{
    private readonly List<GameObject> spirit_list;
    private int current_spirit = 0;

    public MovebarManagement()
    {
        spirit_list = new List<GameObject>();
    }

    public void AddSpiritToFight(GameObject spirit_to_add)
    {
        spirit_list.Add(spirit_to_add);
    }

    public GameObject GetFirstSpirit()
    {
        GameObject spirit_to_move = spirit_list[current_spirit];

        //Update index
        current_spirit++;

        if (current_spirit >= spirit_list.Count)
        {
            current_spirit = 0;
        }

        return (spirit_to_move);
    }
}
