using UnityEngine;

public class MainManagement : MonoBehaviour
{
    [SerializeField] BattleProgressionManagement _BattleProgressionManagement = null;
    [SerializeField] MapManagement _MazeManagement = null;

    [SerializeField] GameObject _MazePanel = null;
    [SerializeField] GameObject _BattlePanel = null;

    [SerializeField] CameraMovement _CameraMovement = null;

    /// <summary>
    /// Switch th panel, and enable the battle
    /// </summary>
    public void TriggerBattle()
    {
        Camera.main.orthographicSize = 12;

        _MazeManagement.SetMapVisibility(false);
        
        _MazePanel.SetActive(false);
        _BattlePanel.SetActive(true);

        _CameraMovement.EnableCameraMovement(false);

        _BattleProgressionManagement.TriggerEncounter();
    }

    /// <summary>
    /// Switch the panel, and show the map
    /// </summary>
    public void ShowMap()
    {
        Camera.main.orthographicSize = 10;

        _BattlePanel.SetActive(false);
        _MazePanel.SetActive(true);

        _CameraMovement.EnableCameraMovement(true);

        _MazeManagement.SetMapVisibility(true);
    }
}
