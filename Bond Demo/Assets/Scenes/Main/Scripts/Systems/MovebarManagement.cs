using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovebarManagement
{
    private readonly List<GameObject> _spirit_list;
    private int _current_spirit = 0;

    public MovebarManagement()
    {
        _spirit_list = new List<GameObject>();
    }

    public void AddSpiritToFight(GameObject spirit_to_add)
    {
        _spirit_list.Add(spirit_to_add);
    }

    public GameObject GetFirstSpirit()
    {
        GameObject spirit_to_move = _spirit_list[_current_spirit];

        //Update index
        _current_spirit++;

        if (_current_spirit >= _spirit_list.Count)
        {
            _current_spirit = 0;
        }

        return (spirit_to_move);
    }
}
