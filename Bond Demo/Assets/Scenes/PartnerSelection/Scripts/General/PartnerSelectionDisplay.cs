using UnityEngine;
using UnityEngine.UI;

public class PartnerSelectionDisplay : MonoBehaviour
{
    private enum DetailText
    {
        Name = 0,
        Health = 1,
        Attack = 2,
        Defense = 3,
        Speed = 4
    }

    [SerializeField] private GameObject _SelectionPanel = null;
    [SerializeField] private GameObject _DetailPanel = null;

    [SerializeField] private GameObject _SpiritImageObject = null;
    [SerializeField] private GameObject _DetailTableObject = null;

    [SerializeField] private Text _PlaceHolderNameText = null;

    [SerializeField] private GameObject _SettingPanel = null;
    [SerializeField] private GameObject _SettingButton = null;

    private BaseSpirit _SelectedSpirit = null;

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
        GeneralComponent.SetSprite(_SpiritImageObject, AssetsLoader.Assets.LoadSprite(_SelectedSpirit.ImageName, LoadEnum.SpiritImage));

        // Set texts for the spirit's data
        GeneralComponent.SetText(GeneralGameObject.GetChildGameObject(_DetailTableObject, (int)DetailText.Name), "Name: " + _SelectedSpirit.Name);
        GeneralComponent.SetText(GeneralGameObject.GetChildGameObject(_DetailTableObject, (int)DetailText.Health), "Health: " + _SelectedSpirit.Health);
        GeneralComponent.SetText(GeneralGameObject.GetChildGameObject(_DetailTableObject, (int)DetailText.Attack), "Attack: " + _SelectedSpirit.Attack);
        GeneralComponent.SetText(GeneralGameObject.GetChildGameObject(_DetailTableObject, (int)DetailText.Defense), "Defense: " + _SelectedSpirit.Defense);
        GeneralComponent.SetText(GeneralGameObject.GetChildGameObject(_DetailTableObject, (int)DetailText.Speed), "Speed: " + _SelectedSpirit.Speed);
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
            _PlaceHolderNameText.text = NameGenerator.GetName();

            GeneralGameObject.ActivateObject(_DetailPanel);
        }
        else
        {
            GeneralGameObject.DeactivateObject(_DetailPanel);
        }
    }

    /// <summary>
    /// Set the visibility for the panel
    /// </summary>
    /// <param name="is_active"></param>
    public void SetUpSelectionPanel(bool is_active)
    {
        if (is_active)
        {
            GeneralGameObject.ActivateObject(_SelectionPanel);
        }
        else
        {
            GeneralGameObject.DeactivateObject(_SelectionPanel);
        }
    }

    public void SetSettingPanel(bool is_active)
    {
        if (is_active)
        {
            GeneralGameObject.ActivateObject(_SettingPanel);
            GeneralGameObject.ActivateObject(_SettingButton);
        }
        else
        {
            GeneralGameObject.DeactivateObject(_SettingPanel);
            GeneralGameObject.DeactivateObject(_SettingButton);
        }
    }
}