using UnityEngine;

public class SettingPanelDisplay : MonoBehaviour
{
    private static GameObject _SettingPanel = null;
    private static bool on_display = false;

    /// <summary>
    /// Create a setting panel if it is not in scene already
    /// </summary>
    public static void SetSettingPanel()
    {
        string prefab_name_with_path;
        GameObject canvas;

        // If the setting panel is set, create a instance of the prefab from the Resources Folder
        if (_SettingPanel == null)
        {
            canvas = GameObject.Find("SettingCanvas");

            if (canvas == null)
            {
                // Find the main canvas in the scene
                canvas = GameObject.Find("Canvas");
            }

            prefab_name_with_path = "Prefab/System/SettingPanel";

            // Create an instance of the setting panel
            _SettingPanel = GameObject.Instantiate(Resources.Load<GameObject>(prefab_name_with_path), canvas.transform);
        }
        else
        {
            if (on_display)
            {
                _SettingPanel.Deactivate();
            }
            else
            {
                _SettingPanel.Activate();
            }

            on_display = !on_display;
        }
    }
}
