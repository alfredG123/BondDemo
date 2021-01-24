using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManagement : MonoBehaviour
{
    [SerializeField] BattleProgressionManagement _BattleProgressionManagement = null;
    [SerializeField] MazeManagement _MazeManagement = null;

    [SerializeField] GameObject _MazePanel = null;
    [SerializeField] GameObject _BattlePanel = null;

    public void TriggerBattle()
    {
        _MazeManagement.SetMapVisibility(false);
        
        _MazePanel.SetActive(false);
        _BattlePanel.SetActive(true);

        _BattleProgressionManagement.TriggerEncounter();
    }

    public void ShowMap()
    {
        _BattlePanel.SetActive(false);
        _MazePanel.SetActive(true);

        _MazeManagement.SetMapVisibility(true);
    }
}
