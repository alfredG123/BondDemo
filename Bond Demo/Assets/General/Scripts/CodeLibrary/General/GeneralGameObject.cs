using UnityEngine;

public static class GeneralGameObject
{
    /// <summary>
    /// Activate the object
    /// </summary>
    /// <param name="object_to_set"></param>
    public static void ActivateObject(GameObject object_to_set)
    {
        SetUpObject(object_to_set, true);
    }

    /// <summary>
    /// Deactivate the object
    /// </summary>
    /// <param name="object_to_set"></param>
    public static void DeactivateObject(GameObject object_to_set)
    {
        SetUpObject(object_to_set, false);
    }
    
    /// <summary>
    /// Activate or deactivate the object
    /// </summary>
    /// <param name="object_to_set"></param>
    /// <param name="is_active"></param>
    private static void SetUpObject(GameObject object_to_set, bool is_active)
    {
        object_to_set.SetActive(is_active);
    }

    /// <summary>
    /// Get the child from the game object at the specified index
    /// </summary>
    /// <param name="object_to_set"></param>
    /// <param name="child_index"></param>
    /// <returns></returns>
    public static GameObject GetChildObject(this GameObject object_to_set, int child_index)
    {
        return(object_to_set.transform.GetChild(child_index).gameObject);
    }
}
