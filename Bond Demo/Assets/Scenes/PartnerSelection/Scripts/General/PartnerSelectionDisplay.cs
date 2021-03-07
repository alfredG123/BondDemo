using UnityEngine;
using UnityEngine.UI;

public class PartnerSelectionDisplay : MonoBehaviour
{
    private enum StartingSpirit
    {
        A1 = 0,
        B1 = 1,
        C1 = 2,
        D1 = 3,
        E1 = 4
    }

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
        int min_value = 0;
        int max_value = 4;

        // If the current play mode is testing, check the parameter
        if (GeneralSetting.CurrentMode == GeneralSetting.Mode.Testing)
        {
            GeneralError.CheckIfLess(spirit_index, min_value, "SetSelectedSpirit");
            GeneralError.CheckIfGreater(spirit_index, max_value, "SetSelectedSpirit");
        }

        if (spirit_index == (int)StartingSpirit.A1)
        {
            _SelectedSpirit = BaseSpirit.A1;
        }
        else if (spirit_index == (int)StartingSpirit.B1)
        {
            _SelectedSpirit = BaseSpirit.B1;
        }
        else if (spirit_index == (int)StartingSpirit.C1)
        {
            _SelectedSpirit = BaseSpirit.C1;
        }
        else if (spirit_index == (int)StartingSpirit.D1)
        {
            _SelectedSpirit = BaseSpirit.D1;
        }
        else if (spirit_index == (int)StartingSpirit.E1)
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
        GeneralComponent.SetSprite(_SpiritImageObject, AssetsLoader.Assets.LoadSprite(_SelectedSpirit.ImageName, LoadObjectEnum.SpiritImage));

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
    /// Activate or deactivate the selection panel
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
}