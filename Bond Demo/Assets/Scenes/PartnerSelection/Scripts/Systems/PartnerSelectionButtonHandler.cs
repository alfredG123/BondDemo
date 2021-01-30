using UnityEngine;
using UnityEngine.UI;

public class PartnerSelectionButtonHandler : MonoBehaviour
{
    [SerializeField] PlayerManagement _PlayerObject = null;
    [SerializeField] PartnerSelectionUIHandler _UIHandler = null;

    [SerializeField] GameObject _Nametext = null;
    [SerializeField] GameObject _PlaceHolderText = null;

    /// <summary>
    /// If the decision is confirmed, assign the spirit to the player, and load the main scene
    /// </summary>
    public void ConfirmSelection()
    {
        string name;

        BaseSpirit spirit = _UIHandler.GetSelectedSpirit();

        name = _Nametext.GetComponent<Text>().text;

        if (string.IsNullOrEmpty(name))
        {
            name = _PlaceHolderText.GetComponent<Text>().text;
        }

        _PlayerObject.SetSpiritAsPartner(spirit, name);

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
