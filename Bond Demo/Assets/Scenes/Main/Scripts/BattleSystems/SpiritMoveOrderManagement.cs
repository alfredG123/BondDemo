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
            prefab = spirit.GetComponent<SpiritPrefab>();

            if (max < prefab.Spirit.Speed + prefab.MoveToPerform.Priority * 1000)
            {
                max = prefab.Spirit.Speed + prefab.MoveToPerform.Priority * 1000;

                spirit_to_move = spirit;
            }
        }

        _SpiritObjectList.Remove(spirit_to_move);

        return (spirit_to_move);
    }

    public void ClearMoveOrder()
    {
        _SpiritObjectList.Clear();
    }
}
