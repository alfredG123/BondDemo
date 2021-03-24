using UnityEngine;
using UnityEngine.UI;

public class PartnerSelectionEventHandlers : MonoBehaviour
{
    [SerializeField] private GameObject _InstructionText = null;
    [SerializeField] private GameObject _StallingButton = null;

    [SerializeField] private Text _SpiritNameText = null;
    [SerializeField] private Text _SpiritNamePlaceHolderText = null;

    private PartnerSelectionDisplay _Display = null;

    /// <summary>
    /// Set up globarl variables
    /// </summary>
    private void Awake()
    {
        _Display = GetComponent<PartnerSelectionDisplay>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowSettingPanel();
        }
    }

    /// <summary>
    /// Display the starting spirits
    /// </summary>
    public void StartSelection()
    {
        _InstructionText.SetText("Choose a spirit as your partner.");

        GeneralGameObject.DeactivateObject(_StallingButton);

        _Display.DisplayStartingSpirits();
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
        PlayerInformation.AddSpiritToParty(spirit, spirit_name);

        // Load the main scene 
        GeneralScene.LoadScene(GeneralScene.Scene.Main);
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
        SettingPanelDisplay.SetSettingPanel();
    }
}
