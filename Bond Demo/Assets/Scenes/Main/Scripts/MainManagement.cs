using UnityEngine;

public class MainManagement : MonoBehaviour
{
    [SerializeField] BattleProgressionManagement _BattleProgressionManagement = null;
    [SerializeField] MazeManagement _MazeManagement = null;

    [SerializeField] GameObject _MazePanel = null;
    [SerializeField] GameObject _BattlePanel = null;

    /// <summary>
    /// Switch th panel, and enable the battle
    /// </summary>
    public void TriggerBattle()
    {
        _MazeManagement.SetMapVisibility(false);
        
        _MazePanel.SetActive(false);
        _BattlePanel.SetActive(true);

        _BattleProgressionManagement.TriggerEncounter();
    }

    /// <summary>
    /// Switch the panel, and show the map
    /// </summary>
    public void ShowMap()
    {
        _BattlePanel.SetActive(false);
        _MazePanel.SetActive(true);

        _MazeManagement.SetMapVisibility(true);
    }
}
