using UnityEngine;
using UnityEngine.UI;

public class PartnerSelectionSceneDisplayHandler : MonoBehaviour
{
    [SerializeField] private GameObject _SelectionPanel = null;
    [SerializeField] private GameObject _DetailPanel = null;

    [SerializeField] private GameObject _SpiritImageObject = null;
    [SerializeField] private GameObject _DetailTableObject = null;

    [SerializeField] private Text _PlaceHolderNameText = null;

    [SerializeField] private PartnerSelectionSceneSpriteCollection _SpiritSpriteCollection = null;

    private BaseSpirit _SelectedSpirit = null;

    private readonly string[] _RandomNickname = { "Max", "Buddy", "Milo", "Toby", "Bella", "Molly", "Ruby", "Lucy" };

    /// <summary>
    /// Display UI to show info about the selected spirit
    /// </summary>
    /// <param name="spirit_index"></param>
    public void DisplaySelectedSpirit(int spirit_index)
    {
        SetSelectedSpirit(spirit_index);

        DisplaySelectedSpiritInfo();

        SetUpSelectionPanel(false);

        SetUpDetailPanel(true);
    }

    /// <summary>
    /// Return the selected spirit
    /// </summary>
    /// <returns></returns>
    public BaseSpirit GetSelectedSpirit()
    {
        return (_SelectedSpirit);
    }

    /// <summary>
    /// Set a spirit as selected based on the index
    /// </summary>
    /// <param name="spirit_index"></param>
    private void SetSelectedSpirit(int spirit_index)
    {
        if (spirit_index == 0)
        {
            _SelectedSpirit = BaseSpirit.A1;
        }
        else if (spirit_index == 1)
        {
            _SelectedSpirit = BaseSpirit.B1;
        }
        else if (spirit_index == 2)
        {
            _SelectedSpirit = BaseSpirit.C1;
        }
        else if (spirit_index == 3)
        {
            _SelectedSpirit = BaseSpirit.D1;
        }
        else if (spirit_index == 4)
        {
            _SelectedSpirit = BaseSpirit.E1;
        }
    }

    /// <summary>
    /// Modified the image and text for showing related information about the chosen spirit
    /// </summary>
    /// <param name="spirit"></param>
    private void DisplaySelectedSpiritInfo()
    {
        // Set sprite for the spirit
        General.SetSprite(_SpiritImageObject, _SpiritSpriteCollection.GetSpiritSpriteByImageName(_SelectedSpirit.ImageName));

        // Set texts for the spirit's data
        General.SetText(_DetailTableObject.transform.GetChild(0).gameObject, "Name: " + _SelectedSpirit.Name);
        General.SetText(_DetailTableObject.transform.GetChild(1).gameObject, "Health: " + _SelectedSpirit.Health);
        General.SetText(_DetailTableObject.transform.GetChild(2).gameObject, "Attack: " + _SelectedSpirit.Attack);
        General.SetText(_DetailTableObject.transform.GetChild(3).gameObject, "Defense: " + _SelectedSpirit.Defense);
        General.SetText(_DetailTableObject.transform.GetChild(4).gameObject, "Speed: " + _SelectedSpirit.Speed);
    }

    /// <summary>
    /// Set the visibility for the panel
    /// </summary>
    /// <param name="is_active"></param>
    public void SetUpDetailPanel(bool is_active)
    {
        // If the detail panel is active, set the placeholder text with a random name
        if (is_active)
        {
            _PlaceHolderNameText.text = _RandomNickname[Random.Range(0, _RandomNickname.Length)];
        }

        General.SetUpObject(_DetailPanel, is_active);
    }

    /// <summary>
    /// Set the visibility for the panel
    /// </summary>
    /// <param name="is_active"></param>
    public void SetUpSelectionPanel(bool is_active)
    {
        General.SetUpObject(_SelectionPanel, is_active);
    }
}