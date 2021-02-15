using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManagement : MonoBehaviour
{
    public struct InventoryItem
    {
        public Item _Item;
        public int _Quantity;
    }

    public List<Spirit> Party { get; private set; } = new List<Spirit>();
    public List<InventoryItem> Bag { get; private set; } = new List<InventoryItem>();

    public void AddSpiritToParty(BaseSpirit spirit_to_set, string name)
    {
        Party.Add(new Spirit(spirit_to_set, name, true));
    }

    public void AddItemToBag(Item item_to_add, int quantity)
    {
        InventoryItem item;

        if((Bag.Count > 0) && (Bag.Any(x=>x._Item == item_to_add)))
        {
            item = Bag.Where(x => x._Item == item_to_add).FirstOrDefault();

            item._Quantity += quantity;
        }
        else
        {
            item = new InventoryItem
            {
                _Item = item_to_add,
                _Quantity = quantity
            };

            Bag.Add(item);
        }
    }

    public void SetUpTemporaryParty()
    {
        Party.Add(new Spirit(BaseSpirit.C1, "Max", true));
        Party.Add(new Spirit(BaseSpirit.D1, "Lax", true));
        Party.Add(new Spirit(BaseSpirit.A1, "Rax", true));
    }
}
