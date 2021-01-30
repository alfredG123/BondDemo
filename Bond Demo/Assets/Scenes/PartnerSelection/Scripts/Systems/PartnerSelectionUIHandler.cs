using UnityEngine;
using UnityEngine.UI;

public class PartnerSelectionUIHandler : MonoBehaviour
{
    [SerializeField] GameObject _SelectionPanel = null;
    [SerializeField] GameObject _DetailPanel = null;

    [SerializeField] GameObject _SpiritImage = null;
    [SerializeField] GameObject _DetailTable = null;

    [SerializeField] SpiritSpriteCollection _SpiritSpriteCollection = null;

    [SerializeField] GameObject _PlaceHolderText = null;

    private BaseSpirit _SelectedSpirit = null;

    private string[] _RandomNickname = { "Max", "Buddy", "Milo", "Toby", "Bella", "Molly", "Ruby", "Lucy" };

    /// <summary>
    /// Display UI to show info about the selected spirit
    /// </summary>
    /// <param name="spirit_index"></param>
    public void DisplaySelectedSpirit(int spirit_index)
    {
        SetSelectedSpirit(spirit_index);

        DisplaySelectedSpiritInfo();

        SetSelectionPanelVisibility(false);

        SetDetailPanelVisibility(true);
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
        _SpiritImage.GetComponent<Image>().sprite = _SpiritSpriteCollection.GetSpiritSpriteByImageName(_SelectedSpirit.ImageName);

        _DetailTable.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Name: " + _SelectedSpirit.Name;
        _DetailTable.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Health: " + _SelectedSpirit.Health;
        _DetailTable.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Attack: " + _SelectedSpirit.Attack;
        _DetailTable.transform.GetChild(3).gameObject.GetComponent<Text>().text = "Defense: " + _SelectedSpirit.Defense;
        _DetailTable.transform.GetChild(4).gameObject.GetComponent<Text>().text = "Speed: " + _SelectedSpirit.Speed;
    }

    /// <summary>
    /// Set the visibility for the panel
    /// </summary>
    /// <param name="is_visible"></param>
    public void SetDetailPanelVisibility(bool is_visible)
    {
        if (is_visible)
        {
            _PlaceHolderText.GetComponent<Text>().text = _RandomNickname[Random.Range(0, _RandomNickname.Length)];
        }

        _DetailPanel.SetActive(is_visible);
    }

    /// <summary>
    /// Set the visibility for the panel
    /// </summary>
    /// <param name="is_visible"></param>
    public void SetSelectionPanelVisibility(bool is_visible)
    {
        _SelectionPanel.SetActive(is_visible);
    }
}