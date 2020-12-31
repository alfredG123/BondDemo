using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovebarManagement
{
    private List<GameObject> _spirit_list;
    private int _current_spirit = 0;

    public MovebarManagement()
    {
        _spirit_list = new List<GameObject>();
    }

    public void AddSpiritToFight(GameObject spirit_to_add)
    {
        int position = 0;
        
        for (int i = 0; i < _spirit_list.Count; i++)
        {
            if (_spirit_list[i].GetComponent<SpiritPrefab>().Spirit.Speed == spirit_to_add.GetComponent<SpiritPrefab>().Spirit.Speed)
            {
                position = i;
                break;
            }
        }

        _spirit_list.Insert(position, spirit_to_add);
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
