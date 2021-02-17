using UnityEngine;

public class MainManagement : MonoBehaviour
{
    [SerializeField] BattleProgressionManagement _BattleProgressionManagement = null;
    [SerializeField] MapManagement _MazeManagement = null;
    [SerializeField] CystalTempleDisplayHandlers _CystalTempleDisplayHanlders = null;

    // Panels
    [SerializeField] GameObject _MazePanel = null;
    [SerializeField] GameObject _BattlePanel = null;
    [SerializeField] GameObject _CystalTemplePanel = null;

    /// <summary>
    /// Switch th panel to the battle, and set up for the battle
    /// </summary>
    public void TriggerBattle(int enemy_count)
    {
        SetUpForNewPanel();

        General.ActivateObject(_BattlePanel);

        _BattleProgressionManagement.TriggerEncounter(enemy_count);
    }

    /// <summary>
    ///  Switch th panel to the cystal temple
    /// </summary>
    public void EnterCystalTemple()
    {
        SetUpForNewPanel();

        General.ActivateObject(_CystalTemplePanel);

        _CystalTempleDisplayHanlders.DisplayTemple();
    }
   
    /// <summary>
    /// Switch the panel to the map
    /// </summary>
    public void ShowMap()
    {
        SetUpForMapPanel();

        General.ActivateObject(_MazePanel);
    }

    /// <summary>
    /// Deactivate map related objects, and adjust the camera
    /// </summary>
    private void SetUpForNewPanel()
    {
        Camera.main.orthographicSize = 12;

        General.DeactivateObject(_MazePanel);

        _MazeManagement.SetUpMapPanel(false);
    }

    /// <summary>
    /// Activate map related object, and adjust the camera
    /// </summary>
    private void SetUpForMapPanel()
    {
        Camera.main.orthographicSize = 10;

        General.DeactivateObject(_BattlePanel);
        General.DeactivateObject(_CystalTemplePanel);
        
        _MazeManagement.SetUpMapPanel(true);
    }
}
