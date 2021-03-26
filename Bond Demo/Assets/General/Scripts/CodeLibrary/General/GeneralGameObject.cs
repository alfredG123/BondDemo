using UnityEngine;

public static class GeneralGameObject
{
    /// <summary>
    /// Activate the object
    /// </summary>
    /// <param name="object_to_set"></param>
    public static void Activate(this GameObject object_to_set)
    {
        object_to_set.SetActive(true);
    }

    /// <summary>
    /// Deactivate the object
    /// </summary>
    /// <param name="object_to_set"></param>
    public static void Deactivate(this GameObject object_to_set)
    {
        object_to_set.SetActive(false);
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
