using UnityEngine;

public class CystalTempleButtonHandlers : MonoBehaviour
{
    [SerializeField] private MainManagement _MainManagement = null;
    [SerializeField] private CystalTempleDisplayHandlers _CystalTempleDisplayHandlers = null;
    [SerializeField] private Canvas _MessageCanvas = null;

    private Spirit _CurrentSpirit = null;

    /// <summary>
    /// Button Handler for switching the panel for showing the map
    /// </summary>
    public void BackToMap()
    {
        _CystalTempleDisplayHandlers.ResetTemple();

        _MainManagement.ShowMap();
    }

    public void UpgradeMove1()
    {
        UpgradeMove(0);
    }

    public void UpgradeMove2()
    {
        UpgradeMove(1);
    }

    public void UpgradeMove3()
    {
        UpgradeMove(2);
    }

    public void UpgradeMove4()
    {
        UpgradeMove(3);
    }

    public void UpgradeMove5()
    {
        UpgradeMove(4);
    }

    private void UpgradeMove(int move_index)
    {
        if (PlayerInformation.UseItem(Item.Cystal, _CurrentSpirit.GetMove(move_index).UpgradeCost))
        {
            _CurrentSpirit.UpgradeMove(move_index);
        }
        else
        {
            TextUIPopUp.CreateTextPopUp("Insufficient", GeneralInput.GetMousePositionInWorldSpace(), Color.red, _MessageCanvas);
        }

        _CystalTempleDisplayHandlers.SelectSpiritMove(_CurrentSpirit);
    }

    public void DisplaySelectSpirit()
    {
        _CystalTempleDisplayHandlers.DisplaySelectSpirit();
    }

    public void SelectSpirit(int spirit_index)
    {
        _CurrentSpirit = PlayerInformation.GetPartyMember(spirit_index);

        _CystalTempleDisplayHandlers.SelectSpiritMove(_CurrentSpirit);
    }
}
