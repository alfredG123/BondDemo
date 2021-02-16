using UnityEngine;
using System.Linq;

public class CystalTempleDisplayHandlers : MonoBehaviour
{
    [SerializeField] GameObject _CystalText = null;
    [SerializeField] PlayerManagement _PlayerManagement = null;

    public void DisplayTemple()
    {
        string cystal_count = "Cystal x";
        PlayerManagement.InventoryItem item = _PlayerManagement.Bag.Where(x => x._Item == Item.Cystal).FirstOrDefault();

        if (item._Item == Item.Cystal)
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
