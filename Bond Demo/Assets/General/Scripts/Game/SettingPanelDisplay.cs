using UnityEngine;

public class SettingPanelDisplay : MonoBehaviour
{
    private static GameObject _SettingPanel = null;

    /// <summary>
    /// Create a setting panel if it is not in scene already
    /// </summary>
    public static void CreateSettingPanel()
    {
        string prefab_name_with_path;
        GameObject main_canvas;

        // If the setting panel is set, create a instance of the prefab from the Resources Folder
        if (_SettingPanel == null)
        {
            // Find the main canvas in the scene
            main_canvas = GameObject.Find("Canvas");

            prefab_name_with_path = "Prefab/System/SettingPanel";

            // Create an instance of the setting panel
            _SettingPanel = GameObject.Instantiate(Resources.Load<GameObject>(prefab_name_with_path), main_canvas.transform);
        }
    }

    /// <summary>
    /// Activate the setting panel
    /// </summary>
    public static void DisplaySettingPanel()
    {
        GeneralGameObject.ActivateObject(_SettingPanel);
    }

    /// <summary>
    /// Deactivate the setting panel
    /// </summary>
    public static void HideSettingPanel()
    {
        GeneralGameObject.DeactivateObject(_SettingPanel);
    }
}
