using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerSelectionButtonHandler : MonoBehaviour
{
    [SerializeField] PlayerManagement _PlayerObject = null;
    [SerializeField] PartnerSelectionUIHandler _UIHandler = null;

    /// <summary>
    /// If the decision is confirmed, assign the spirit to the player, and load the main scene
    /// </summary>
    public void ConfirmSelection()
    {
        BaseSpirit spirit = _UIHandler.GetSelectedSpirit();

        _PlayerObject.SetSpiritAsPartner(spirit);

        // Load the main scene 
        General.LoadScene(TypeScene.Main);
    }

    /// <summary>
    /// If the decision is cancelled, change the panel
    /// </summary>
    public void Rethink()
    {
        _UIHandler.SetDetailPanelVisibility(false);

        _UIHandler.SetSelectionPanelVisibility(true);
    }
}
