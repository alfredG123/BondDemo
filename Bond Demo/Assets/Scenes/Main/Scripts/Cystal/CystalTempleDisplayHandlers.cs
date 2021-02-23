using UnityEngine;
using System.Linq;

public class CystalTempleDisplayHandlers : MonoBehaviour
{
    [SerializeField] GameObject _CystalText = null;

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
    }
}
