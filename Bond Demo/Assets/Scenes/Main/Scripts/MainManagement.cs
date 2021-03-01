using UnityEngine;

public class MainManagement : MonoBehaviour
{
    [SerializeField] private BattleProgressionManagement _BattleProgressionManagement = null;
    [SerializeField] private MapManagement _MapManagement = null;
    [SerializeField] private CystalTempleDisplayHandlers _CystalTempleDisplayHanlders = null;
    [SerializeField] private RestPanelDisplayHandlers _RestPanelDisplayHandlers = null;
    [SerializeField] private TreasurePanelDisplayHandlers _TreasurePanelDisplayHandlers = null;
    [SerializeField] private SurvivedSpiritPanelDisplayHandlers _SurvivedSpiritPanelDisplayHandlers = null;

    // Panels
    [SerializeField] private GameObject _MapPanel = null;
    [SerializeField] private GameObject _BattlePanel = null;
    [SerializeField] private GameObject _CystalTemplePanel = null;
    [SerializeField] private GameObject _RestPanel = null;
    [SerializeField] private GameObject _TreasurePanel = null;
    [SerializeField] private GameObject _SurvivedSpiritPanel = null;
    [SerializeField] private GameObject _LosePanel = null;

    [SerializeField] private GameObject _SettingPanel = null;
    [SerializeField] private GameObject _SettingButton = null;

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

    public void TakeRest()
    {
        SetUpForNewPanel();

        General.ActivateObject(_RestPanel);

        _RestPanelDisplayHandlers.DisplayRest();
    }

    public void GetTreasure()
    {
        SetUpForNewPanel();

        General.ActivateObject(_TreasurePanel);

        _TreasurePanelDisplayHandlers.DisplayTreasure();
    }

    public void MeetSpirit()
    {
        SetUpForNewPanel();

        General.ActivateObject(_SurvivedSpiritPanel);

        _SurvivedSpiritPanelDisplayHandlers.DisplaySurvivedSpirit();
    }

    public void Lose()
    {
        SetUpForNewPanel();

        General.ActivateObject(_LosePanel);
    }

    /// <summary>
    /// Switch the panel to the map
    /// </summary>
    public void ShowMap()
    {
        SetUpForMapPanel();

        General.ActivateObject(_MapPanel);
    }

    /// <summary>
    /// Deactivate map related objects, and adjust the camera
    /// </summary>
    private void SetUpForNewPanel()
    {
        Camera.main.orthographicSize = 12;

        General.DeactivateObject(_MapPanel);

        _MapManagement.SetUpMapPanel(false);
    }

    /// <summary>
    /// Activate map related object, and adjust the camera
    /// </summary>
    private void SetUpForMapPanel()
    {
        Camera.main.orthographicSize = 10;

        General.DeactivateObject(_BattlePanel);
        General.DeactivateObject(_CystalTemplePanel);
        General.DeactivateObject(_RestPanel);
        General.DeactivateObject(_TreasurePanel);
        General.DeactivateObject(_SurvivedSpiritPanel);

        _MapManagement.SetUpMapPanel(true);
    }

    public void ShowSettingPanel()
    {
        _MapManagement.SetPause(true);

        General.ActivateObject(_SettingPanel);
        General.DeactivateObject(_SettingButton);
    }

    public void HideSettingPanel()
    {
        _MapManagement.SetPause(false);
        
        General.DeactivateObject(_SettingPanel);
        General.ActivateObject(_SettingButton);
    }

    public void ReturnToTitle()
    {
        General.LoadScene(TypeScene.Title);
    }
}
