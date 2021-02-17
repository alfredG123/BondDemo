using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CystalTempleButtonHandlers : MonoBehaviour
{
    [SerializeField] private MainManagement _MainManagement = null;

    /// <summary>
    /// Button Handler for switching the panel for showing the map
    /// </summary>
    public void BackToMap()
    {
        _MainManagement.ShowMap();
    }
}
