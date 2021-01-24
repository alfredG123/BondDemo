using UnityEngine;
using UnityEngine.UI;

public class PartnerSelectionUIDisplay : MonoBehaviour
{
    [SerializeField] GameObject _SelectionPanel = null;
    [SerializeField] GameObject _DetailPanel = null;

    [SerializeField] GameObject _SpiritImage = null;
    [SerializeField] GameObject _DetailTable = null;

    [SerializeField] SpiritSpriteCollection _SpiritSpriteCollection = null;

    [SerializeField] PlayerManagement _PlayerObject = null;

    private BaseSpirit _Spirit;

    /// <summary>
    /// Enable the selection panel
    /// </summary>
    private void Awake()
    {
        _SelectionPanel.SetActive(true);
    }

    /// <summary>
    /// Display UI to show info about the selected spirit
    /// </summary>
    /// <param name="spirit_index"></param>
    public void DisplaySelectedSpiritInfo(int spirit_index)
    {
        if (spirit_index == 0)
        {
            _Spirit = BaseSpirit.A1;
        }
        else if (spirit_index == 1)
        {
            _Spirit = BaseSpirit.B1;
        }
        else if (spirit_index == 2)
        {
            _Spirit = BaseSpirit.C1;
        }
        else if (spirit_index == 3)
        {
            _Spirit = BaseSpirit.D1;
        }
        else if (spirit_index == 4)
        {
            _Spirit = BaseSpirit.E1;
        }

        DisplaySpiritInfo();

        SetSelectionDisplay(false);

        SetDetailPanelDisplay(true);
    }

    /// <summary>
    /// Modified the image and text for showing related information about the chosen spirit
    /// </summary>
    /// <param name="spirit"></param>
    private void DisplaySpiritInfo()
    {
        _SpiritImage.GetComponent<Image>().sprite = _SpiritSpriteCollection.GetSpiritSpriteByImageName(_Spirit.ImageName);

        _DetailTable.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Name: " + _Spirit.Name;
        _DetailTable.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Health: " + _Spirit.Health;
        _DetailTable.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Attack: " + _Spirit.Attack;
        _DetailTable.transform.GetChild(3).gameObject.GetComponent<Text>().text = "Defense: " + _Spirit.Defense;
        _DetailTable.transform.GetChild(4).gameObject.GetComponent<Text>().text = "Speed: " + _Spirit.Speed;
    }

    /// <summary>
    /// If the decision is confirmed, load the main scene
    /// </summary>
    public void ConfirmChoice()
    {
        _PlayerObject.SetSpiritAsPartner(_Spirit);

        // Load the main scene 
        General.LoadScene(TypeScene.Main);
    }

    /// <summary>
    /// If the decision is unconfirmed, change the panel
    /// </summary>
    public void BackButtonEvent()
    {
        SetDetailPanelDisplay(false);

        SetSelectionDisplay(true);
    }

    /// <summary>
    /// Set the visibility for the panel
    /// </summary>
    /// <param name="is_visible"></param>
    private void SetDetailPanelDisplay(bool is_visible)
    {
        _DetailPanel.SetActive(is_visible);
    }

    /// <summary>
    /// Set the visibility for the panel
    /// </summary>
    /// <param name="is_visible"></param>
    private void SetSelectionDisplay(bool is_visible)
    {
        _SelectionPanel.SetActive(is_visible);
    }
}