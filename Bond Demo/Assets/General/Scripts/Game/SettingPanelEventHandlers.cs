using UnityEngine;

public class SettingPanelEventHandlers : MonoBehaviour
{
    /// <summary>
    /// Activate the setting panel
    /// </summary>
    public void SetSettingPanel()
    {
        SettingPanelDisplay.SetSettingPanel();
    }

    /// <summary>
    /// Load the title scene
    /// </summary>
    public void ReturnToTitle()
    {
        GeneralScene.LoadScene(GeneralScene.Scene.Title);
    }
}
