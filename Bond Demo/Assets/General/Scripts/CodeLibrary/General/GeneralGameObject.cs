using UnityEngine;

public static class GeneralGameObject
{
    /// <summary>
    /// Activate the object
    /// </summary>
    /// <param name="object_to_set"></param>
    public static void ActivateObject(GameObject object_to_set)
    {
        GeneralError.CheckIfNull(object_to_set, "ActivateObject");

        SetUpObject(object_to_set, true);
    }

    /// <summary>
    /// Deactivate the object
    /// </summary>
    /// <param name="object_to_set"></param>
    public static void DeactivateObject(GameObject object_to_set)
    {
        GeneralError.CheckIfNull(object_to_set, "DeactivateObject");

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
    public static GameObject GetChildGameObject(GameObject object_to_set, int child_index)
    {
        int first_child_index;

        // If the current mode is testing, check all the parameters
        if (GeneralSetting.IsTestingEnabled())
        {
            first_child_index = 0;

            GeneralError.CheckIfNull(object_to_set, "GetChild");
            GeneralError.CheckIfLess(child_index, first_child_index, "GetChild");
            GeneralError.VerifyChildCount(child_index, object_to_set, "GetChild");
        }

        return(object_to_set.transform.GetChild(child_index).gameObject);
    }
}
