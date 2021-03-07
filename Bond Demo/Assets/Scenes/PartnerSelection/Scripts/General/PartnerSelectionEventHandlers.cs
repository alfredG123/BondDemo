using UnityEngine;
using UnityEngine.UI;

public class PartnerSelectionEventHandlers : MonoBehaviour
{
    [SerializeField] private Text _SpiritNameText = null;
    [SerializeField] private Text _SpiritNamePlaceHolderText = null;

    private PartnerSelectionDisplay _Display = null;

    private void Awake()
    {
        _Display = GetComponent<PartnerSelectionDisplay>();
    }

    /// <summary>
    /// If the decision is confirmed, assign the spirit to the player, and load the main scene
    /// </summary>
    public void ConfirmSelection()
    {
        // Get the spirit that is being selected
        BaseSpirit spirit = _Display.GetSelectedSpirit();

        // Get the name for the spirit
        string spirit_name = GetSpiritName();

        // Add the spirit to the player's party
        PlayerManagement.AddSpiritToParty(spirit, spirit_name);

        // Load the main scene 
        GeneralScene.LoadScene(Scene.Main);
    }

    /// <summary>
    /// Get the name from the text boxes
    /// </summary>
    /// <returns></returns>
    private string GetSpiritName()
    {
        string spirit_name = _SpiritNameText.text;

        // If the spirit name is not set, use the placeholder name
        if (string.IsNullOrEmpty(spirit_name))
        {
            spirit_name = _SpiritNamePlaceHolderText.text;
        }

        return (spirit_name);
    }

    /// <summary>
    /// If the decision is cancelled, change the panel
    /// </summary>
    public void Rethink()
    {
        _Display.SetUpDetailPanel(false);

        _Display.SetUpSelectionPanel(true);
    }

    public void ShowSettingPanel()
    {
        
        _Display.SetSettingPanel(true);
    }

    public void BackToSelection()
    {
        _Display.SetSettingPanel(false);
    }

    public void ReturnToTitle()
    {
        GeneralScene.LoadScene(Scene.Title);
    }
}
