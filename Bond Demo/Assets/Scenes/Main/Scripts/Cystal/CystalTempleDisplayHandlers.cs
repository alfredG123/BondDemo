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

    [SerializeField] private GameObject _SpiritNickname = null;
    [SerializeField] private GameObject _SpiritExample = null;
    [SerializeField] private GameObject _SpiritSelectionGroup = null;
    [SerializeField] private GameObject _NoMoveWarning = null;

    [SerializeField] CystalTempleButtonHandlers _CystalTempleButtonHandlers = null;

    public void DisplayTemple()
    {
        string cystal_count = "Cystal x";
        PlayerInformation.InventoryItem item = PlayerInformation.GetItem(Item.Cystal);

        if (item == null)
        {
            cystal_count += "0";
        }
        else
        {
            cystal_count += item._Quantity;
        }

        _CystalText.SetText(cystal_count);

        DisplayInitialDisplay();
    }

    public void DisplayInitialDisplay()
    {
        _InitialDisplay.Activate();
    }

    public void HideInitialDisplay()
    {
        _InitialDisplay.Deactivate();
    }

    public void DisplaySelectionDisplay()
    {
        _SelectionDisplay.Activate();
    }

    public void HideSelectionDisplay()
    {
        _SelectionDisplay.Deactivate();
    }

    public void DisplayUpgradeGroupDisplay()
    {
        _UpgradeGroup.Activate();
    }

    public void HideUpgradeGroupDisplay()
    {
        _UpgradeGroup.Deactivate();
    }

    public void DisplaySelectSpirit()
    {
        GameObject spirit_button;
        Button button;

        HideInitialDisplay();
        HideUpgradeGroupDisplay();

        DisplaySelectionDisplay();

        if (PlayerInformation.PartyMemberCount() == 0)
        {
            PlayerInformation.SetUpTemporaryPlayer();
        }

        if (_SpiritSelectionGroup.transform.childCount > 1)
        {
            return;
        }

        for (int i = 0; i < PlayerInformation.PartyMemberCount(); i++)
        {
            int spirit_index = i;

            spirit_button = GameObject.Instantiate(_SpiritExample, _SpiritSelectionGroup.transform);
            spirit_button.transform.GetChild(0).gameObject.SetText(PlayerInformation.GetPartyMember(i).Name);
            spirit_button.Activate();
            
            button = spirit_button.GetComponent<Button>();
            
            // spirit_index is passed by reference
            button.onClick.AddListener(() => { _CystalTempleButtonHandlers.SelectSpirit(spirit_index); });
        }
    }

    public void SelectSpiritMove(Spirit spirit)
    {
        string cystal_count = "Cystal x";
        PlayerInformation.InventoryItem item = PlayerInformation.GetItem(Item.Cystal);
        bool has_move = false;

        if (item == null)
        {
            cystal_count += "0";
        }
        else
        {
            cystal_count += item._Quantity;
        }

        _CystalText.SetText(cystal_count);

        HideSelectionDisplay();

        DisplayUpgradeGroupDisplay();

        _SpiritNickname.SetText(spirit.Name);

        _UpgradeGroup.transform.GetChild(1).gameObject.SetSprite(AssetsLoader.Assets.LoadSprite(spirit.ImageName, LoadObjectEnum.SpiritImage));

        for (int i = 0; i < _SkillInfoGroup.transform.childCount; i++)
        {
            if ((i == 0) || (i == 1))
            {
                _SkillInfoGroup.transform.GetChild(i).gameObject.Activate();
                _SkillUpdageButtonGroup.transform.GetChild(i).gameObject.Activate();

                if (i == 0)
                {
                    if (spirit.BasicAttack.IsUpgradeable)
                    {
                        _SkillInfoGroup.transform.GetChild(i).GetChild(0).gameObject.SetText(spirit.BasicAttack.Name);
                        _SkillInfoGroup.transform.GetChild(i).GetChild(1).gameObject.SetText(spirit.BasicAttack.Description);
                        _SkillUpdageButtonGroup.transform.GetChild(i).GetChild(0).gameObject.SetText(spirit.BasicAttack.UpgradeCost.ToString());

                        has_move = true;
                    }
                    else
                    {
                        _SkillInfoGroup.transform.GetChild(i).gameObject.Deactivate();
                        _SkillUpdageButtonGroup.transform.GetChild(i).gameObject.Deactivate();
                    }
                }
                else
                {
                    if (spirit.BasicDefend.IsUpgradeable)
                    {
                        _SkillInfoGroup.transform.GetChild(i).GetChild(0).gameObject.SetText( spirit.BasicDefend.Name);
                        _SkillInfoGroup.transform.GetChild(i).GetChild(1).gameObject.SetText(spirit.BasicDefend.Description);
                        _SkillUpdageButtonGroup.transform.GetChild(i).GetChild(0).gameObject.SetText(spirit.BasicDefend.UpgradeCost.ToString());

                        has_move = true;
                    }
                    else
                    {
                        _SkillInfoGroup.transform.GetChild(i).gameObject.Deactivate();
                        _SkillUpdageButtonGroup.transform.GetChild(i).gameObject.Deactivate();
                    }
                }
            }
            else if ((i - 2) >= spirit.MoveSet.Count)
            {
                _SkillInfoGroup.transform.GetChild(i).gameObject.Deactivate();
                _SkillUpdageButtonGroup.transform.GetChild(i).gameObject.Deactivate();
            }
            else if (spirit.MoveSet[i - 2].IsUpgradeable)
            {
                _SkillInfoGroup.transform.GetChild(i).GetChild(0).gameObject.SetText(spirit.MoveSet[i - 2].Name);
                _SkillInfoGroup.transform.GetChild(i).GetChild(1).gameObject.SetText( spirit.MoveSet[i - 2].Description);
                _SkillUpdageButtonGroup.transform.GetChild(i).GetChild(0).gameObject.SetText(spirit.MoveSet[i - 2].UpgradeCost.ToString());

                _SkillInfoGroup.transform.GetChild(i).gameObject.Activate();
                _SkillUpdageButtonGroup.transform.GetChild(i).gameObject.Activate();

                has_move = true;
            }
            else
            {
                _SkillInfoGroup.transform.GetChild(i).gameObject.Deactivate();
                _SkillUpdageButtonGroup.transform.GetChild(i).gameObject.Deactivate();
            }
        }

        if (has_move)
        {
            _NoMoveWarning.Deactivate();
        }
        else
        {
            _NoMoveWarning.Activate();
        }
    }

    public void ResetTemple()
    {
        _InitialDisplay.Deactivate();
        _SelectionDisplay.Deactivate();
        _UpgradeGroup.Deactivate();
    }
}
