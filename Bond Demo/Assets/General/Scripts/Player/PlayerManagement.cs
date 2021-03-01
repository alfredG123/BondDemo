using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManagement : MonoBehaviour
{
    public class InventoryItem
    {
        public Item _Item;
        public int _Quantity;
    }

    private static List<Spirit> _Party = null;
    private static List<InventoryItem> _Bag = null;

    private void Awake()
    {
        if (_Party == null)
        {
            _Party = new List<Spirit>();
        }

        if (_Bag == null)
        {
            _Bag = new List<InventoryItem>();
        }
    }

    public static void ResetPlayer()
    {
        if (_Party != null)
        {
            _Party.Clear();
        }

        if (_Bag != null)
        {
            _Bag.Clear();
        }
    }

    public static int PartyMemberCount()
    {
        return (_Party.Count);
    }

    public static void RemoveFaintSpirit(Spirit faint_spirit)
    {
        _Party.Remove(faint_spirit);
    }

    public static Spirit GetPartyMember(int spirit_member_index)
    {
        return (_Party[spirit_member_index]);
    }

    public static void AddSpiritToParty(BaseSpirit spirit_to_set, string name)
    {
        _Party.Add(new Spirit(spirit_to_set, name, true));
    }

    public static void AddItemToBag(Item item_to_add, int quantity)
    {
        InventoryItem item = GetItem(item_to_add);

        if (item == null)
        {
            item = new InventoryItem
            {
                _Item = item_to_add,
                _Quantity = quantity
            };

            _Bag.Add(item);
        }
        else
        {
            item._Quantity += quantity;
        }
    }

    public static InventoryItem GetItem(Item item_to_get)
    {
        return (_Bag.Where(x => x._Item == item_to_get).FirstOrDefault());
    }

    public static bool UseItem(Item item_to_use, int quantity)
    {
        bool success = false;
        InventoryItem item = _Bag.Where(x => x._Item == item_to_use).FirstOrDefault();

        if ((item != null) && (item._Quantity >= quantity))
        {
            item._Quantity -= quantity;

            success = true;
        }

        return (success);
    }


    public static void SetUpTemporaryParty()
    {
        _Party.Add(new Spirit(BaseSpirit.C1, "Max1", true));
        _Party.Add(new Spirit(BaseSpirit.D1, "Lax2", true));
        _Party.Add(new Spirit(BaseSpirit.A1, "Rax3", true));
    }
}
