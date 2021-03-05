using UnityEngine;
using UnityEngine.UI;

public class CystalTempleDisplayHandlers : MonoBehaviour
{
    [SerializeField] private GameObject _CystalText = null;
    [SerializeField] private GameObject _SkillInfoGroup = null;
    [SerializeField] private GameObject _SkillUpdageButtonGroup = null;

    [SerializeField] private GameObject _InitialDisplay = null;
    [SerializeField] private GameObject _SelectionDisplay = null;
    [SerializeField] private GameObject _UpgradeGroup = null;

    [SerializeField] private GameObject _SpiritExample = null;
    [SerializeField] private GameObject _SpiritSelectionGroup = null;

    [SerializeField] CystalTempleButtonHandlers _CystalTempleButtonHandlers = null;

    public void DisplayTemple()
    {
        string cystal_count = "Cystal x";
        PlayerManagement.InventoryItem item = PlayerManagement.GetItem(Item.Cystal);

        if (item == null)
        {
            cystal_count += "0";
        }
        else
        {
            cystal_count += item._Quantity;
        }

        GeneralComponent.SetText(_CystalText, cystal_count);

        DisplayInitialDisplay();
    }

    public void DisplayInitialDisplay()
    {
        GeneralGameObject.ActivateObject(_InitialDisplay);
    }

    public void HideInitialDisplay()
    {
        GeneralGameObject.DeactivateObject(_InitialDisplay);
    }

    public void DisplaySelectionDisplay()
    {
        GeneralGameObject.ActivateObject(_SelectionDisplay);
    }

    public void HideSelectionDisplay()
    {
        GeneralGameObject.DeactivateObject(_SelectionDisplay);
    }

    public void DisplayUpgradeGroupDisplay()
    {
        GeneralGameObject.ActivateObject(_UpgradeGroup);
    }

    public void HideUpgradeGroupDisplay()
    {
        GeneralGameObject.DeactivateObject(_UpgradeGroup);
    }

    public void DisplaySelectSpirit()
    {
        GameObject spirit_button;
        Button button;

        HideInitialDisplay();
        HideUpgradeGroupDisplay();

        DisplaySelectionDisplay();

        if (PlayerManagement.PartyMemberCount() == 0)
        {
            PlayerManagement.SetUpTemporaryPlayer();
        }

        if (_SpiritSelectionGroup.transform.childCount > 1)
        {
            return;
        }

        for (int i = 0; i < PlayerManagement.PartyMemberCount(); i++)
        {
            int spirit_index = i;

            spirit_button = GameObject.Instantiate(_SpiritExample, _SpiritSelectionGroup.transform);
            GeneralComponent.SetText(spirit_button.transform.GetChild(0).gameObject, PlayerManagement.GetPartyMember(i).Name);
            GeneralGameObject.ActivateObject(spirit_button);
            
            button = spirit_button.GetComponent<Button>();
            
            // spirit_index is passed by reference
            button.onClick.AddListener(() => { _CystalTempleButtonHandlers.SelectSpirit(spirit_index); });
        }
    }

    public void SelectSpiritMove(Spirit spirit)
    {
        string cystal_count = "Cystal x";
        PlayerManagement.InventoryItem item = PlayerManagement.GetItem(Item.Cystal);

        if (item == null)
        {
            cystal_count += "0";
        }
        else
        {
            cystal_count += item._Quantity;
        }

        GeneralComponent.SetText(_CystalText, cystal_count);

        HideSelectionDisplay();

        DisplayUpgradeGroupDisplay();

        GeneralComponent.SetSprite(_UpgradeGroup.transform.GetChild(0).gameObject, AssetsLoader.Assets.LoadSprite(spirit.ImageName, LoadEnum.SpiritImage));

        for (int i = 0; i < _SkillInfoGroup.transform.childCount; i++)
        {
            if ((i == 0) || (i == 1))
            {
                if (i == 0)
                {
                    GeneralComponent.SetText(_SkillInfoGroup.transform.GetChild(i).GetChild(0).gameObject, spirit.BasicAttack.Name);
                    GeneralComponent.SetText(_SkillInfoGroup.transform.GetChild(i).GetChild(1).gameObject, spirit.BasicAttack.Description);
                    GeneralComponent.SetText(_SkillUpdageButtonGroup.transform.GetChild(i).GetChild(0).gameObject, spirit.BasicAttack.UpgradeCost.ToString());
                }
                else
                {
                    GeneralComponent.SetText(_SkillInfoGroup.transform.GetChild(i).GetChild(0).gameObject, spirit.BasicDefend.Name);
                    GeneralComponent.SetText(_SkillInfoGroup.transform.GetChild(i).GetChild(1).gameObject, spirit.BasicDefend.Description);
                    GeneralComponent.SetText(_SkillUpdageButtonGroup.transform.GetChild(i).GetChild(0).gameObject, spirit.BasicDefend.UpgradeCost.ToString());
                }

                GeneralGameObject.ActivateObject(_SkillInfoGroup.transform.GetChild(i).gameObject);
                GeneralGameObject.ActivateObject(_SkillUpdageButtonGroup.transform.GetChild(i).gameObject);
            }
            else if ((i - 2) >= spirit.MoveSet.Count)
            {
                GeneralGameObject.DeactivateObject(_SkillInfoGroup.transform.GetChild(i).gameObject);
                GeneralGameObject.DeactivateObject(_SkillUpdageButtonGroup.transform.GetChild(i).gameObject);
            }
            else if (spirit.MoveSet[i - 2].IsUpgradeable)
            {
                GeneralComponent.SetText(_SkillInfoGroup.transform.GetChild(i).GetChild(0).gameObject, spirit.MoveSet[i - 2].Name);
                GeneralComponent.SetText(_SkillInfoGroup.transform.GetChild(i).GetChild(1).gameObject, spirit.MoveSet[i - 2].Description);
                GeneralComponent.SetText(_SkillUpdageButtonGroup.transform.GetChild(i).GetChild(0).gameObject, spirit.MoveSet[i - 2].UpgradeCost.ToString());

                GeneralGameObject.ActivateObject(_SkillInfoGroup.transform.GetChild(i).gameObject);
                GeneralGameObject.ActivateObject(_SkillUpdageButtonGroup.transform.GetChild(i).gameObject);
            }
            else
            {
                GeneralGameObject.DeactivateObject(_SkillInfoGroup.transform.GetChild(i).gameObject);
                GeneralGameObject.DeactivateObject(_SkillUpdageButtonGroup.transform.GetChild(i).gameObject);
            }
        }
    }
}
