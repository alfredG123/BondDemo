﻿using System.Collections.Generic;
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
    private static List<Spirit> _ActiveParty = null;

    private void Awake()
    {
        if(_ActiveParty == null)
        {
            _ActiveParty = new List<Spirit>();
        }

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

    public static int ActivePartyMemberCount()
    {
        return (_ActiveParty.Count);
    }

    public static int PartyMemberCount()
    {
        return (_Party.Count);
    }

    public static void RemoveFaintSpirit(Spirit faint_spirit)
    {
        _ActiveParty.Remove(faint_spirit);

        _Party.Remove(faint_spirit);
    }

    public static void ResetActiveMember()
    {
        int index = 0;

        while ((_ActiveParty.Count < 3) && (_ActiveParty.Count < _Party.Count))
        {
            if (_ActiveParty.Contains(_Party[index]))
            {
                _ActiveParty.Add(_Party[index]);
            }

            index++;
        }
    }

    public static Spirit GetActivePartyMember(int spirit_member_index)
    {
        Spirit spirit = null;

        if (spirit_member_index < _ActiveParty.Count)
        {
            spirit = _ActiveParty[spirit_member_index];
        }

        return (spirit);
    }

    public static Spirit GetPartyMember(int spirit_member_index)
    {
        return (_Party[spirit_member_index]);
    }

    public static bool CheckIfActive(Spirit spirit)
    {
        return (_ActiveParty.Contains(spirit));
    }

    public static void AddSpiritToParty(BaseSpirit spirit_to_set, string name)
    {
        Spirit spirit = new Spirit(spirit_to_set, name, true);

        if (_ActiveParty.Count < 3)
        {
            _ActiveParty.Add(spirit);
        }

        _Party.Add(spirit);
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
        AddSpiritToParty(BaseSpirit.C1, "Max1");
        AddSpiritToParty(BaseSpirit.D1, "Lax2");
        AddSpiritToParty(BaseSpirit.A1, "Rax3");
    }
}
