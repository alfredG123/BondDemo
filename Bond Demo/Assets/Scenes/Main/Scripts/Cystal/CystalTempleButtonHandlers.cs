using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CystalTempleButtonHandlers : MonoBehaviour
{
    [SerializeField] private MainManagement _MainManagement = null;
    [SerializeField] private CystalTempleDisplayHandlers _CystalTempleDisplayHandlers = null;

    /// <summary>
    /// Button Handler for switching the panel for showing the map
    /// </summary>
    public void BackToMap()
    {
        _MainManagement.ShowMap();
    }

    public void UpdateSkill1()
    {
        Debug.Log("Update 1");
    }

    public void UpdateSkill2()
    {
        Debug.Log("Update 2");
    }

    public void UpdateSkill3()
    {
        Debug.Log("Update 3");
    }

    public void UpdateSkill4()
    {
        Debug.Log("Update 4");
    }

    public void UpdateSkill5()
    {
        Debug.Log("Update 5");
    }

    public void DisplaySelectSpirit()
    {
        _CystalTempleDisplayHandlers.DisplaySelectSpirit();
    }

    public void SelectSpirt(int spirit_index)
    {
        _CystalTempleDisplayHandlers.SelectSpirt(PlayerManagement.GetPartyMember(spirit_index));
    }
}
