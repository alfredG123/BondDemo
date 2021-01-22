using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManagement : MonoBehaviour
{
    [SerializeField] BattleProgressionManagement _BattleProgressionManagement = null;
    [SerializeField] MazeManagement _MazeManagement = null;
    
    public void TriggerBattle()
    {
        _MazeManagement.SetMapVisibility(false);

        _BattleProgressionManagement.TriggerEncounter();
    }

    public void ShowMap()
    {
        _MazeManagement.SetMapVisibility(true);
    }
}
