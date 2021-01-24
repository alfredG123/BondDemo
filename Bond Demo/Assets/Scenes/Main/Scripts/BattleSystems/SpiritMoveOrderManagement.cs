using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpiritMoveOrderManagement
{
    /*
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
            General.ReturnToTitleSceneForErrors("MovebarManagement.AddSpiritObjectToList", "The game object does not have the script, SpiritPrefab.");
        }

        // Add the game object to the list
        _spirit_object_list.Add(spirit_object_to_add);

        // Sort the list
        _spirit_object_list = _spirit_object_list.OrderByDescending(spirit_object => General.GetSpiritPrefabComponent(spirit_object).Spirit.Speed).ToList();
    }

    public bool HasSpiritToMove()
    {
        bool has_spirit_to_move = true;

        if (_current_spirit_object_index >= _spirit_object_list.Count)
        {
            has_spirit_to_move = false;
        }

        return (has_spirit_to_move);
    }

    public void RemoveFaintSpirit(GameObject faint_spirit)
    {
        _spirit_object_list.Remove(faint_spirit);
    }

    public void SortList()
    {
        // Sort the list
        _spirit_object_list = _spirit_object_list.OrderByDescending(spirit_object => General.GetSpiritPrefabComponent(spirit_object).Spirit.Speed).ToList();
    }

    public void SetUpMoveOrder()
    {
        _current_spirit_object_index = 0;
    }

    /// <summary>
    /// Get the spirit based on the index, current_spirit
    /// </summary>
    /// <returns></returns>
    public GameObject GetSpiritToMove()
    {
        // If the index exceeds the list range, reset the index
        if (_current_spirit_object_index >= _spirit_object_list.Count)
        {
            General.ReturnToTitleSceneForErrors("SpiritMoveOrderManagement.GetSpiritToMove", "_current_spirit_object_index is too large");
        }

        GameObject spirit_to_move = _spirit_object_list[_current_spirit_object_index];

        // Update the index
        _current_spirit_object_index++;

        return (spirit_to_move);
    }

    public void SetPriorityForDefense(GameObject spirit)
    {
        int index = _spirit_object_list.IndexOf(spirit);

        _spirit_object_list.RemoveAt(index);

        _spirit_object_list.Insert(0, spirit);
    }
    */
}
