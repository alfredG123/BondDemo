using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpiritMoveOrderManagement
{
    private List<GameObject> _spirit_object_list;
    private int _current_spirit_object_index;

    /// <summary>
    /// Initialize the global variables
    /// </summary>
    public SpiritMoveOrderManagement()
    {
        _spirit_object_list = new List<GameObject>();

        _current_spirit_object_index = 0;
    }

    public int Count
    {
        get => _spirit_object_list.Count;
    }

    /// <summary>
    /// Add the specific game object to the list
    /// </summary>
    /// <param name="spirit_object_to_add"></param>
    public void AddSpiritObjectToList(GameObject spirit_object_to_add)
    {
        // Check if the object has the required script
        if (spirit_object_to_add.GetComponent<SpiritPrefab>() == null)
        {
            GeneralScripts.ReturnToTitleSceneForErrors("MovebarManagement.AddSpiritObjectToList", "The game object does not have the script, SpiritPrefab.");
        }

        // Add the game object to the list
        _spirit_object_list.Add(spirit_object_to_add);

        // Sort the list
        _spirit_object_list = _spirit_object_list.OrderByDescending(spirit_object => GeneralScripts.GetSpiritPrefabScript(spirit_object).Spirit.Speed).ToList();
    }

    /// <summary>
    /// Get the spirit based on the index, current_spirit
    /// </summary>
    /// <returns></returns>
    public GameObject GetSpiritToMove()
    {
        GameObject spirit_to_move = _spirit_object_list[_current_spirit_object_index];

        // Update the index
        _current_spirit_object_index++;

        // If the index exceeds the list range, reset the index
        if (_current_spirit_object_index >= _spirit_object_list.Count)
        {
            _current_spirit_object_index = 0;
        }

        return (spirit_to_move);
    }
}
