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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowSettingPanel();
        }
    }

    /// <summary>
    /// Switch th panel to the battle, and set up for the battle
    /// </summary>
    public void TriggerBattle(int enemy_count)
    {
        SetUpForNewPanel();

        _BattlePanel.Activate();

        _BattleProgressionManagement.TriggerEncounter(enemy_count);
    }

    /// <summary>
    ///  Switch th panel to the cystal temple
    /// </summary>
    public void EnterCystalTemple()
    {
        SetUpForNewPanel();

        _CystalTemplePanel.Activate();

        _CystalTempleDisplayHanlders.DisplayTemple();
    }

    public void TakeRest()
    {
        SetUpForNewPanel();

        _RestPanel.Activate();

        _RestPanelDisplayHandlers.DisplayRest();
    }

    public void GetTreasure()
    {
        SetUpForNewPanel();

        _TreasurePanel.Activate();

        _TreasurePanelDisplayHandlers.DisplayTreasure();
    }

    public void MeetSpirit()
    {
        SetUpForNewPanel();

        _SurvivedSpiritPanel.Activate();

        _SurvivedSpiritPanelDisplayHandlers.DisplaySurvivedSpirit();
    }

    public void Lose()
    {
        SetUpForNewPanel();

        _LosePanel.Activate();
    }

    /// <summary>
    /// Switch the panel to the map
    /// </summary>
    public void ShowMap()
    {
        SetUpForMapPanel();

        _MapPanel.Activate();
    }

    /// <summary>
    /// Deactivate map related objects, and adjust the camera
    /// </summary>
    private void SetUpForNewPanel()
    {
        Camera.main.orthographicSize = 12;

        _MapPanel.Deactivate();

        _MapManagement.SetUpMapPanel(false);
    }

    /// <summary>
    /// Activate map related object, and adjust the camera
    /// </summary>
    private void SetUpForMapPanel()
    {
        Camera.main.orthographicSize = 10;

        _BattlePanel.Deactivate();
        _CystalTemplePanel.Deactivate();
        _RestPanel.Deactivate();
        _TreasurePanel.Deactivate();
        _SurvivedSpiritPanel.Deactivate();

        _MapManagement.SetUpMapPanel(true);
    }

    public void ShowSettingPanel()
    {
        _MapManagement.SetPause(true);

        SettingPanelDisplay.SetSettingPanel();
    }

    public void ReturnToTitle()
    {
        GeneralScene.LoadScene(GeneralScene.Scene.Title);
    }
}
