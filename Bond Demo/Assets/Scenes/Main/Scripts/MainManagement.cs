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

        GeneralGameObject.ActivateObject(_BattlePanel);

        _BattleProgressionManagement.TriggerEncounter(enemy_count);
    }

    /// <summary>
    ///  Switch th panel to the cystal temple
    /// </summary>
    public void EnterCystalTemple()
    {
        SetUpForNewPanel();

        GeneralGameObject.ActivateObject(_CystalTemplePanel);

        _CystalTempleDisplayHanlders.DisplayTemple();
    }

    public void TakeRest()
    {
        SetUpForNewPanel();

        GeneralGameObject.ActivateObject(_RestPanel);

        _RestPanelDisplayHandlers.DisplayRest();
    }

    public void GetTreasure()
    {
        SetUpForNewPanel();

        GeneralGameObject.ActivateObject(_TreasurePanel);

        _TreasurePanelDisplayHandlers.DisplayTreasure();
    }

    public void MeetSpirit()
    {
        SetUpForNewPanel();

        GeneralGameObject.ActivateObject(_SurvivedSpiritPanel);

        _SurvivedSpiritPanelDisplayHandlers.DisplaySurvivedSpirit();
    }

    public void Lose()
    {
        SetUpForNewPanel();

        GeneralGameObject.ActivateObject(_LosePanel);
    }

    /// <summary>
    /// Switch the panel to the map
    /// </summary>
    public void ShowMap()
    {
        SetUpForMapPanel();

        GeneralGameObject.ActivateObject(_MapPanel);
    }

    /// <summary>
    /// Deactivate map related objects, and adjust the camera
    /// </summary>
    private void SetUpForNewPanel()
    {
        Camera.main.orthographicSize = 12;

        GeneralGameObject.DeactivateObject(_MapPanel);

        _MapManagement.SetUpMapPanel(false);
    }

    /// <summary>
    /// Activate map related object, and adjust the camera
    /// </summary>
    private void SetUpForMapPanel()
    {
        Camera.main.orthographicSize = 10;

        GeneralGameObject.DeactivateObject(_BattlePanel);
        GeneralGameObject.DeactivateObject(_CystalTemplePanel);
        GeneralGameObject.DeactivateObject(_RestPanel);
        GeneralGameObject.DeactivateObject(_TreasurePanel);
        GeneralGameObject.DeactivateObject(_SurvivedSpiritPanel);

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
