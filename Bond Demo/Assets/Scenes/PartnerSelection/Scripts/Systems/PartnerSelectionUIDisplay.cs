using UnityEngine;
using UnityEngine.UI;

public class PartnerSelectionUIDisplay : MonoBehaviour
{
    [SerializeField] GameObject _SelectionPanel = null;
    [SerializeField] GameObject _DetailPanel = null;

    [SerializeField] GameObject _SpiritImage = null;
    [SerializeField] GameObject _DetailTable = null;

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
        BaseSpirit spirit = null;

        if (spirit_index == 0)
        {
            spirit = BaseSpirit.A1;
        }
        else if (spirit_index == 1)
        {
            spirit = BaseSpirit.B1;
        }
        else if (spirit_index == 2)
        {
            spirit = BaseSpirit.C1;
        }
        else if (spirit_index == 3)
        {
            spirit = BaseSpirit.D1;
        }
        else if (spirit_index == 4)
        {
            spirit = BaseSpirit.E1;
        }

        DisplaySpiritInfo(spirit);

        SetSelectionDisplay(false);

        SetDetailPanelDisplay(true);
    }

    /// <summary>
    /// Modified the image and text for showing related information about the chosen spirit
    /// </summary>
    /// <param name="spirit"></param>
    private void DisplaySpiritInfo(BaseSpirit spirit)
    {
        _SpiritImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(spirit.GetImageNameWithPath());

        _DetailTable.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Name: " + spirit.Name;
        _DetailTable.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Health: " + spirit.Health;
        _DetailTable.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Attack: " + spirit.Attack;
        _DetailTable.transform.GetChild(3).gameObject.GetComponent<Text>().text = "Defense: " + spirit.Defense;
        _DetailTable.transform.GetChild(4).gameObject.GetComponent<Text>().text = "Speed: " + spirit.Speed;
    }

    /// <summary>
    /// If the decision is confirmed, load the main scene
    /// </summary>
    public void ConfirmChoice()
    {
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