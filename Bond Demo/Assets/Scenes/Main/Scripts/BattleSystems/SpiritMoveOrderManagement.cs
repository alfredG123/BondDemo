using System.Collections.Generic;
using UnityEngine;

public class SpiritMoveOrderManagement
{
    private readonly List<GameObject> _SpiritObjectList = new List<GameObject>();

    /// <summary>
    /// Default Constructor
    /// </summary>
    public SpiritMoveOrderManagement()
    {
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
        _SpiritObjectList.Add(spirit_object_to_add);
    }

    public bool HasSpiritToMove()
    {
        return (_SpiritObjectList.Count > 0);
    }

    /// <summary>
    /// Get the spirit based on the index, current_spirit
    /// </summary>
    /// <returns></returns>
    public GameObject GetSpiritToMove()
    {
        GameObject spirit_to_move = null;
        SpiritPrefab prefab;
        float max = 0;

        // If the index exceeds the list range, reset the index
        if (_SpiritObjectList.Count <= 0)
        {
            General.ReturnToTitleSceneForErrors("SpiritMoveOrderManagement.GetSpiritToMove", "no more spirit");
        }

        foreach (GameObject spirit in _SpiritObjectList)
        {
            if (spirit.activeSelf)
            {
                prefab = spirit.GetComponent<SpiritPrefab>();

                if (max < prefab.Spirit.Speed + prefab.MoveToPerform.Priority * 1000)
                {
                    max = prefab.Spirit.Speed + prefab.MoveToPerform.Priority * 1000;

                    spirit_to_move = spirit;
                }
            }
        }

        _SpiritObjectList.Remove(spirit_to_move);

        return (spirit_to_move);
    }

    public void ClearMoveOrder()
    {
        _SpiritObjectList.Clear();
    }

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
