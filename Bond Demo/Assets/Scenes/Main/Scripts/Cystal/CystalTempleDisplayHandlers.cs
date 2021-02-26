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

        General.SetText(_CystalText, cystal_count);

        DisplayInitialDisplay();
    }

    public void DisplayInitialDisplay()
    {
        General.ActivateObject(_InitialDisplay);
    }

    public void HideInitialDisplay()
    {
        General.DeactivateObject(_InitialDisplay);
    }

    public void DisplaySelectionDisplay()
    {
        General.ActivateObject(_SelectionDisplay);
    }

    public void HideSelectionDisplay()
    {
        General.DeactivateObject(_SelectionDisplay);
    }

    public void DisplayUpgradeGroupDisplay()
    {
        General.ActivateObject(_UpgradeGroup);
    }

    public void HideUpgradeGroupDisplay()
    {
        General.DeactivateObject(_UpgradeGroup);
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
            PlayerManagement.SetUpTemporaryParty();
        }

        if (_SpiritSelectionGroup.transform.childCount > 1)
        {
            return;
        }

        for (int i = 0; i < PlayerManagement.PartyMemberCount(); i++)
        {
            int spirit_index = i;

            spirit_button = GameObject.Instantiate(_SpiritExample, _SpiritSelectionGroup.transform);
            General.SetText(spirit_button.transform.GetChild(0).gameObject, PlayerManagement.GetPartyMember(i).Name);
            General.ActivateObject(spirit_button);
            
            button = spirit_button.GetComponent<Button>();
            
            // spirit_index is passed by reference
            button.onClick.AddListener(() => { _CystalTempleButtonHandlers.SelectSpirt(spirit_index); });
        }
    }

    public void SelectSpirtMove(Spirit spirit)
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

        General.SetText(_CystalText, cystal_count);

        HideSelectionDisplay();

        DisplayUpgradeGroupDisplay();

        General.SetSprite(_UpgradeGroup.transform.GetChild(0).gameObject, AssetsLoader.Assets.LoadSprite(spirit.ImageName, LoadEnum.SpiritImage));

        for (int i = 0; i < _SkillInfoGroup.transform.childCount; i++)
        {
            if ((i == 0) || (i == 1))
            {
                if (i == 0)
                {
                    General.SetText(_SkillInfoGroup.transform.GetChild(i).GetChild(0).gameObject, spirit.BasicAttack.Name);
                    General.SetText(_SkillInfoGroup.transform.GetChild(i).GetChild(1).gameObject, spirit.BasicAttack.Description);
                    General.SetText(_SkillUpdageButtonGroup.transform.GetChild(i).GetChild(0).gameObject, spirit.BasicAttack.UpgradeCost.ToString());
                }
                else
                {
                    General.SetText(_SkillInfoGroup.transform.GetChild(i).GetChild(0).gameObject, spirit.BasicDefend.Name);
                    General.SetText(_SkillInfoGroup.transform.GetChild(i).GetChild(1).gameObject, spirit.BasicDefend.Description);
                    General.SetText(_SkillUpdageButtonGroup.transform.GetChild(i).GetChild(0).gameObject, spirit.BasicDefend.UpgradeCost.ToString());
                }

                General.ActivateObject(_SkillInfoGroup.transform.GetChild(i).gameObject);
                General.ActivateObject(_SkillUpdageButtonGroup.transform.GetChild(i).gameObject);
            }
            else if ((i - 2) >= spirit.MoveSet.Count)
            {
                General.DeactivateObject(_SkillInfoGroup.transform.GetChild(i).gameObject);
                General.DeactivateObject(_SkillUpdageButtonGroup.transform.GetChild(i).gameObject);
            }
            else if (spirit.MoveSet[i - 2].IsUpgradeable)
            {
                General.SetText(_SkillInfoGroup.transform.GetChild(i).GetChild(0).gameObject, spirit.MoveSet[i - 2].Name);
                General.SetText(_SkillInfoGroup.transform.GetChild(i).GetChild(1).gameObject, spirit.MoveSet[i - 2].Description);
                General.SetText(_SkillUpdageButtonGroup.transform.GetChild(i).GetChild(0).gameObject, spirit.MoveSet[i - 2].UpgradeCost.ToString());

                General.ActivateObject(_SkillInfoGroup.transform.GetChild(i).gameObject);
                General.ActivateObject(_SkillUpdageButtonGroup.transform.GetChild(i).gameObject);
            }
            else
            {
                General.DeactivateObject(_SkillInfoGroup.transform.GetChild(i).gameObject);
                General.DeactivateObject(_SkillUpdageButtonGroup.transform.GetChild(i).gameObject);
            }
        }
    }
}
