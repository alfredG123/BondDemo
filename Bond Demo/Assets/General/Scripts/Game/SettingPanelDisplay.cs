using UnityEngine;

public class SettingPanelDisplay : MonoBehaviour
{
    private static GameObject _SettingPanel = null;

    public static void CreateSettingPanel()
    {
        string prefab_name_with_path;
        GameObject main_canvas;

        // Find the main canvas in the scene
        main_canvas = GameObject.Find("Canvas");

        // If the setting panel is set, create a instance of the prefab from the Resources Folder
        if (_SettingPanel == null)
        {
            prefab_name_with_path = "Prefab/System/SettingPanel";

            GameObject.Instantiate(Resources.Load<GameObject>(prefab_name_with_path), main_canvas.transform);
        }
    }

    public void DisplaySettingPanel()
    {

    }

    public void HideSettingPanel()
    {

    }
}
