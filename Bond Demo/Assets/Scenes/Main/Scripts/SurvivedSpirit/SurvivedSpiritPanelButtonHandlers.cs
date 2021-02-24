using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivedSpiritPanelButtonHandlers : MonoBehaviour
{
    [SerializeField] private MainManagement _MainManagement = null;
    [SerializeField] private SurvivedSpiritPanelDisplayHandlers _SurvivedSpiritPanelDisplayHandlers = null;

    /// <summary>
    /// Button Handler for switching the panel for showing the map
    /// </summary>
    public void BackToMap()
    {
        _MainManagement.ShowMap();
    }

    public void AskForHelp()
    {
        _SurvivedSpiritPanelDisplayHandlers.DisplayHelpSupply();
    }

    public void ConfirmHelp()
    {
        _SurvivedSpiritPanelDisplayHandlers.HideResult();

        BackToMap();
    }

    public void InviteSpirit()
    {
        _SurvivedSpiritPanelDisplayHandlers.DisplayInvitationResult();
    }
}
