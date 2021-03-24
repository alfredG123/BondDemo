using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

    [SerializeField] private GameObject _StartingSpiritGroup = null;

    [SerializeField] private GameObject _SpiritImageObject = null;
    [SerializeField] private GameObject _DetailTableObject = null;

    [SerializeField] private Text _PlaceHolderNameText = null;

    private readonly List<int> _StartingSpiritIndexList = new List<int>();
    private int _SelectedSpiritIndex = 0;

    /// <summary>
    /// Display three random spirits
    /// </summary>
    public void DisplayStartingSpirits()
    {
        int available_spirit_count = 3;
        GameObject spirit_button_object;
        Button spirit_button;
        int text_object_index = 0;
        int first_spirit_index = 0;

        SetUpSpiritIndexList();

        for (int i = 0; i < available_spirit_count; i++)
        {
            int spirit_index;

            // Get a random spirit
            spirit_index = _StartingSpiritIndexList[GeneralRandom.GetRandomNumberInRange(first_spirit_index, _StartingSpiritIndexList.Count)];
            _StartingSpiritIndexList.Remove(spirit_index);

            // Create a button for the spirit
            spirit_button_object = GameObject.Instantiate(AssetsLoader.Assets.LoadGameObject("SpiritButton", LoadObjectEnum.Button), _StartingSpiritGroup.transform);

            // Set the button text to the spirit's name
            GeneralGameObject.GetChildGameObject(spirit_button_object, text_object_index).SetText(GetStartingSpirit(spirit_index).Name);

            // Get the button component
            spirit_button = GeneralComponent.GetButton(spirit_button_object);

            // spirit_index is passed by reference
            spirit_button.onClick.AddListener(() => { DisplaySelectedSpirit(spirit_index); });
        }
    }

    /// <summary>
    /// Add pre-set spirit indexes into the list
    /// </summary>
    private void SetUpSpiritIndexList()
    {
        _StartingSpiritIndexList.Add(0);
        _StartingSpiritIndexList.Add(1);
        _StartingSpiritIndexList.Add(2);
        _StartingSpiritIndexList.Add(3);
        _StartingSpiritIndexList.Add(4);
    }

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
        return (GetStartingSpirit(_SelectedSpiritIndex));
    }

    /// <summary>
    /// Return the starting spirit by index
    /// </summary>
    /// <param name="spirit_index"></param>
    /// <returns></returns>
    private BaseSpirit GetStartingSpirit(int spirit_index)
    {
        BaseSpirit spirit = null;

        if (spirit_index == (int)StartingSpirit.A1)
        {
            spirit = BaseSpirit.A1;
        }
        else if (spirit_index == (int)StartingSpirit.B1)
        {
            spirit = BaseSpirit.B1;
        }
        else if (spirit_index == (int)StartingSpirit.C1)
        {
            spirit = BaseSpirit.C1;
        }
        else if (spirit_index == (int)StartingSpirit.D1)
        {
            spirit = BaseSpirit.D1;
        }
        else if (spirit_index == (int)StartingSpirit.E1)
        {
            spirit = BaseSpirit.E1;
        }

        return (spirit);
    }

    /// <summary>
    /// Set a spirit as selected based on the index
    /// </summary>
    /// <param name="spirit_index"></param>
    private void SetSelectedSpirit(int spirit_index)
    {
        _SelectedSpiritIndex = spirit_index;
    }

    /// <summary>
    /// Modified the image and text for showing related information about the chosen spirit
    /// </summary>
    /// <param name="spirit"></param>
    private void DisplaySelectedSpiritInfo()
    {
        BaseSpirit selected_spirit = GetSelectedSpirit();

        // Set sprite for the spirit
        GeneralComponent.SetSprite(_SpiritImageObject, AssetsLoader.Assets.LoadSprite(selected_spirit.ImageName, LoadObjectEnum.SpiritImage));

        // Set texts for the spirit's data
        GeneralGameObject.GetChildGameObject(_DetailTableObject, (int)DetailText.Name).SetText("Name: " + selected_spirit.Name);
        GeneralGameObject.GetChildGameObject(_DetailTableObject, (int)DetailText.Health).SetText("Health: " + selected_spirit.Health);
        GeneralGameObject.GetChildGameObject(_DetailTableObject, (int)DetailText.Attack).SetText("Attack: " + selected_spirit.Attack);
        GeneralGameObject.GetChildGameObject(_DetailTableObject, (int)DetailText.Defense).SetText("Defense: " + selected_spirit.Defense);
        GeneralGameObject.GetChildGameObject(_DetailTableObject, (int)DetailText.Speed).SetText("Speed: " + selected_spirit.Speed);
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