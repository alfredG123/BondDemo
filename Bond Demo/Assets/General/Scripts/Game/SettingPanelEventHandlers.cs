using UnityEngine;

public class SettingPanelEventHandlers : MonoBehaviour
{
    /// <summary>
    /// Set up a setting panel in the scene
    /// </summary>
    private void Awake()
    {
        SettingPanelDisplay.CreateSettingPanel();
    }

    /// <summary>
    /// Activate the setting panel
    /// </summary>
    public void DisplaySettingPanel()
    {
        SettingPanelDisplay.DisplaySettingPanel();
    }

    /// <summary>
    /// Deactivate the setting panel
    /// </summary>
    public void HideSettingPanel()
    {
        SettingPanelDisplay.HideSettingPanel();
    }

    /// <summary>
    /// Load the title scene
    /// </summary>
    public void ReturnToTitle()
    {
        GeneralScene.LoadScene(GeneralScene.Scene.Title);
    }
}
